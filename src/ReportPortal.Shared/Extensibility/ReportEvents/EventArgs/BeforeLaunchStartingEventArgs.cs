using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    public class BeforeLaunchStartingEventArgs : ReportEventBaseArgs
    {
        public BeforeLaunchStartingEventArgs(IClientService clientService, IConfiguration configuration, StartLaunchRequest startLaunchRequest) : base(clientService, configuration)
        {
            StartLaunchRequest = startLaunchRequest;
        }

        public StartLaunchRequest StartLaunchRequest { get; }
    }
}
