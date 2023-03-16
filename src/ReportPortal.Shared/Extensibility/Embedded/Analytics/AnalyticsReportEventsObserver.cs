using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Internal.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.Embedded.Analytics
{
    /// <summary>
    /// Google Analytics launch events tracker.
    /// </summary>
    public class AnalyticsReportEventsObserver : IReportEventsObserver, IDisposable
    {
        private const string MEASUREMENT_ID = "UA-173456809-1";
        private const string BASE_URI = "https://www.google-analytics.com";
        private const string CLIENT_NAME = "commons-dotnet";

        private static ITraceLogger TraceLogger => TraceLogManager.Instance.GetLogger<AnalyticsReportEventsObserver>();

        private readonly string _clientId;

        private readonly string _clientVersion;

        private readonly string _platformVersion;

        private HttpClient _httpClient;
        private readonly object _httpClientLock = new object();

        public AnalyticsReportEventsObserver()
        {
            _clientId = Guid.NewGuid().ToString();

            // Client is this assembly
            _clientVersion = typeof(AnalyticsReportEventsObserver).Assembly.GetName().Version.ToString(3);

#if NETSTANDARD
            _platformVersion = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
#else
            _platformVersion = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
#endif
        }

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
            if (string.IsNullOrEmpty(agentName) || string.IsNullOrEmpty(agentVersion))
            {
                var agentAssemblyName = Assembly.GetCallingAssembly().GetName();
                AgentName = agentAssemblyName.Name;
                _agentVersion = agentAssemblyName.Version.ToString(3);
            }
            else
            {
                AgentName = agentName;
                _agentVersion = agentVersion;
            }
        }

        public static string AgentName { get; private set; } = "Anonymous";

        private static string _agentVersion;
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
                var category = $"Client name \"{CLIENT_NAME}\", version \"{_clientVersion}\", interpreter \"{_platformVersion}\"";
                var label = $"Agent name \"{AgentName}\", version \"{AgentVersion}\"";

                var requestData = $"/collect?v=1&tid={MEASUREMENT_ID}&cid={_clientId}&t=event&ec={category}&ea=Start launch&el={label}";

                var httpClient = GetHttpClient(args.Configuration);
                
                // schedule tracking request
                _sendGaUsageTask = Task.Run(async () =>
                {
                    try
                    {
                        using (var response = await httpClient.PostAsync(requestData, null))
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
