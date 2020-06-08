using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Internal.Logging;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Extensibility.Analytics
{
    /// <summary>
    /// Google Analytics launch events tracker.
    /// </summary>
    public class AnalyticsReportEventsObserver : IReportEventsObserver, IDisposable
    {
        private const string MEASUREMENT_ID = "UA-168688323-1";
        private const string BASE_URI = "https://www.google-analytics.com";

        private static ITraceLogger TraceLogger => TraceLogManager.Instance.GetLogger<AnalyticsReportEventsObserver>();

        private readonly string _clientId;

        private readonly string _clientName;
        private readonly string _clientVersion;

        private HttpClient _httpClient;

        public AnalyticsReportEventsObserver()
        {
            _clientId = Guid.NewGuid().ToString();

            // Client is this assembly
            _clientName = "dotnet-shared";
            _clientVersion = typeof(AnalyticsReportEventsObserver).Assembly.GetName().Version.ToString(3);

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BASE_URI);
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

        public static string AgentName { get; private set; } = "Unknown";

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

        public void Initialize(IReportEventsSource reportEventsSource)
        {
            _reportEventsSource = reportEventsSource;
            reportEventsSource.OnBeforeLaunchStarting += ReportEventsSource_OnBeforeLaunchStarting;
        }

        private void ReportEventsSource_OnBeforeLaunchStarting(Reporter.ILaunchReporter launchReporter, ReportEvents.EventArgs.BeforeLaunchStartingEventArgs args)
        {
            if (args.Configuration.GetValue("Analytics:Enabled", true))
            {
                var category = $"Client name \"{_clientName}\", version \"{_clientVersion}\"";
                var label = $"Agent name \"{AgentName}\", version \"{AgentVersion}\"";

                var requestData = $"/collect?v=1&tid={MEASUREMENT_ID}&cid={_clientId}&t=event&ec={category}&ea=Start launch&el={label}";

                // schedule tracking request and forget
                Task.Run(async () =>
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

        public void Dispose()
        {
            if (_reportEventsSource != null)
            {
                _reportEventsSource.OnBeforeLaunchStarting -= ReportEventsSource_OnBeforeLaunchStarting;
            }

            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }
        }
    }
}
