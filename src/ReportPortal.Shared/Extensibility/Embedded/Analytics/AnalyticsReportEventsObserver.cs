using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Internal.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Extensibility.Embedded.Analytics
{
    /// <summary>
    /// Google Analytics launch events tracker.
    /// </summary>
    public class AnalyticsReportEventsObserver : IReportEventsObserver, IDisposable
    {
        private const string CLIENT_INFO = "Ry1XUDU3UlNHOFhMOkVGaGFqc2J3U3RTbmEtc0NydGN6RHc=";
        private const string BASE_URI = "https://www.google-analytics.com";
        private const string CLIENT_NAME = "commons-dotnet";
        private const string EVENT_NAME = "start_launch";

        private static ITraceLogger TraceLogger => TraceLogManager.Instance.GetLogger<AnalyticsReportEventsObserver>();

        private readonly string _clientVersion;

        private readonly string _platformVersion;

        private readonly string _measurementId;
        private readonly string _apiKey;

        private HttpClient _httpClient;
        private readonly object _httpClientLock = new object();

        /// <summary>
        /// Create an instance of AnalyticsReportEventsObserver object, construct own HttpClient if neccessary.
        /// </summary>
        public AnalyticsReportEventsObserver()
        {
            // Client is this assembly
            _clientVersion = typeof(AnalyticsReportEventsObserver).Assembly.GetName().Version.ToString(3);

#if NETSTANDARD
            _platformVersion = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
#else
            _platformVersion = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
#endif
            var clientInfo = Encoding.UTF8.GetString(Convert.FromBase64String(CLIENT_INFO)).Split(':');
            _measurementId = clientInfo[0];
            _apiKey = clientInfo[1];
        }

        /// <summary>
        /// Create an instance of AnalyticsReportEventsObserver object, use provided HttpMessageHandler to construct an HttpClient.
        /// </summary>
        /// <param name="httpHandler">Http handler to construc a client</param>
        public AnalyticsReportEventsObserver(HttpMessageHandler httpHandler) : this()
        {
            _httpClient = new HttpClient(httpHandler)
            {
                BaseAddress = new Uri(BASE_URI)
            };
        }

        /// <summary>
        /// Sets custom information about agent name and version. It's expected this method is invoked on agent side.
        /// </summary>
        /// <param name="agentName">Human readable name of the agent.</param>
        /// <param name="agentVersion">Automatically identified as calling assembly version if null.</param>
        public static void DefineConsumer(string agentName, string agentVersion = null)
        {
            // determine agent name
            if (string.IsNullOrEmpty(agentName))
            {
                var agentAssemblyName = Assembly.GetCallingAssembly().GetName();
                AgentName = agentAssemblyName.Name;
            }
            else
            {
                AgentName = agentName;
            }

            // determine agent version
            if (string.IsNullOrEmpty(agentVersion))
            {
                var agentAssemblyName = Assembly.GetCallingAssembly().GetName();
                _agentVersion = agentAssemblyName.Version.ToString(3);
            }
            else
            {
                _agentVersion = agentVersion;
            }
        }

        /// <summary>
        /// Set the name of the Agent or use default name "Anonymous".
        /// </summary>
        /// <returns>The Agent name.</returns>
        public static string AgentName { get; private set; } = "Anonymous";

        private static string _agentVersion;
        /// <summary>
        /// Return the version of the Agent retrieved from Assembly.
        /// </summary>
        /// <returns>The Agent version.</returns>
        public static string AgentVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_agentVersion))
                {
                    var agentAssemblyName = Assembly.GetCallingAssembly().GetName();
                    _agentVersion = agentAssemblyName.Version.ToString(3);
                }

                return _agentVersion;
            }
        }

        private IReportEventsSource _reportEventsSource;

        /// <inheritdoc />
        public void Initialize(IReportEventsSource reportEventsSource)
        {
            _reportEventsSource = reportEventsSource ?? throw new ArgumentNullException(nameof(reportEventsSource));
            reportEventsSource.OnBeforeLaunchStarting += ReportEventsSource_OnBeforeLaunchStarting;
            reportEventsSource.OnAfterLaunchFinished += ReportEventsSource_OnAfterLaunchFinished;
        }

        private Task _sendGaUsageTask;

        HttpClient GetHttpClient(IConfiguration configuration)
        {
            if (_httpClient != null)
                return _httpClient;

            lock (_httpClientLock)
            {
                if (_httpClient != null)
                    return _httpClient;

                var handler = new HttpClientHandler();
                var ignoreSslErrors = configuration.GetValue<bool>("Server:IgnoreSslErrors", false);

#if NET462
                if (ignoreSslErrors)
                {
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                }
#else
                if (ignoreSslErrors)
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                }
#endif
                _httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(BASE_URI)
                };
            }

            return _httpClient;
        }

        private void ReportEventsSource_OnBeforeLaunchStarting(Reporter.ILaunchReporter launchReporter, ReportEvents.EventArgs.BeforeLaunchStartingEventArgs args)
        {
            if (args.Configuration.GetValue("Analytics:Enabled", true))
            {
                // schedule tracking request
                _sendGaUsageTask = Task.Run(async () =>
                {
                    var requestParams = new Dictionary<string, string>() {
                        { "client_name", CLIENT_NAME },
                        { "client_version", _clientVersion },
                        { "interpreter", _platformVersion },
                        { "agent_name", AgentName },
                        { "agent_version", AgentVersion }
                    };

                    var eventData = new Dictionary<string, object>()
                    {
                        { "name", EVENT_NAME },
                        { "params", requestParams }
                    };

                    var requestUri = $"/mp/collect?measurement_id={_measurementId}&api_secret={_apiKey}";

                    var httpClient = GetHttpClient(args.Configuration);

                    var payload = new Dictionary<string, object>()
                    {
                        { "client_id", await ClientIdProvider.GetClientIdAsync() },
                        { "events", new List<object> { eventData } }
                    };

                    string content;

                    using (var stream = new MemoryStream())
                    {
                        await JsonSerializer.SerializeAsync(stream, payload, payload.GetType());
                        stream.Position = 0;
                        using (var reader = new StreamReader(stream))
                        {
                            content = await reader.ReadToEndAsync();
                        }
                    }

                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                    try
                    {
                        using (var response = await httpClient.PostAsync(requestUri, stringContent))
                        {
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    catch (Exception exp)
                    {
                        TraceLogger.Error($"Cannot track OnBeforeLaunchStarting event: {exp}");
                    }
                });
            }
        }

        private void ReportEventsSource_OnAfterLaunchFinished(Reporter.ILaunchReporter launchReporter, ReportEvents.EventArgs.AfterLaunchFinishedEventArgs args)
        {
            _sendGaUsageTask?.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Release HttpClient if needed.
        /// </summary>
        public void Dispose()
        {
            if (_reportEventsSource != null)
            {
                _reportEventsSource.OnBeforeLaunchStarting -= ReportEventsSource_OnBeforeLaunchStarting;
                _reportEventsSource.OnAfterLaunchFinished -= ReportEventsSource_OnAfterLaunchFinished;
            }

            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }
        }
    }
}
