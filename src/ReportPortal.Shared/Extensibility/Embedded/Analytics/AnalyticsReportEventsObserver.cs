using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Internal.Logging;
using System;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        private const string PLATFORM_VERSION_PATTERN = @"^(\d+\.\d+)";

        private static ITraceLogger TraceLogger => TraceLogManager.Instance.GetLogger<AnalyticsReportEventsObserver>();

        private readonly string _clientId;

        private readonly string _clientVersion;

        private readonly string _platformVersion;

        private HttpClient _httpClient;

        public AnalyticsReportEventsObserver() : this(new HttpClientHandler
        {
#if !NET45 && !NET46
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
#endif
        })
        {
        }

        public AnalyticsReportEventsObserver(HttpMessageHandler httpHandler)
        {
            _clientId = Guid.NewGuid().ToString();

            // Client is this assembly
            _clientVersion = typeof(AnalyticsReportEventsObserver).Assembly.GetName().Version.ToString(3);

#if NETSTANDARD
            _platformVersion = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
#else
            _platformVersion = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
#endif

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
            AgentName = agentName;

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

        IReportEventsSource _reportEventsSource;

        /// <inheritdoc />
        public void Initialize(IReportEventsSource reportEventsSource)
        {
            _reportEventsSource = reportEventsSource;
            reportEventsSource.OnBeforeLaunchStarting += ReportEventsSource_OnBeforeLaunchStarting;
            reportEventsSource.OnAfterLaunchFinished += ReportEventsSource_OnAfterLaunchFinished;
        }

        private Task _sendGaUsageTask;

        private void ReportEventsSource_OnBeforeLaunchStarting(Reporter.ILaunchReporter launchReporter, ReportEvents.EventArgs.BeforeLaunchStartingEventArgs args)
        {
            if (args.Configuration.GetValue("Analytics:Enabled", true))
            {
                var category = $"Client name \"{CLIENT_NAME}\", version \"{_clientVersion}\", interpreter \"{_platformVersion}\"";
                var label = $"Agent name \"{AgentName}\", version \"{AgentVersion}\"";

                var requestData = $"/collect?v=1&tid={MEASUREMENT_ID}&cid={_clientId}&t=event&ec={category}&ea=Start launch&el={label}";

                // schedule tracking request
                _sendGaUsageTask = Task.Run(async () =>
                {
                    try
                    {
                        var response = await _httpClient.PostAsync(requestData, null);
                        response.EnsureSuccessStatusCode();
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
            if (_sendGaUsageTask != null)
            {
                try
                {
                    _sendGaUsageTask.GetAwaiter().GetResult();
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// Release HtpClient if needed.
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
