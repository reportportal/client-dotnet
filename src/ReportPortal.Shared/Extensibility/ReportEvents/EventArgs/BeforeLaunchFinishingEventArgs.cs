using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    public class BeforeLaunchFinishingEventArgs : ReportEventBaseArgs
    {
        public BeforeLaunchFinishingEventArgs(IClientService clientService, IConfiguration configuration, FinishLaunchRequest finishLaunchRequest) : base(clientService, configuration)
        {
            FinishLaunchRequest = finishLaunchRequest;
        }

        public FinishLaunchRequest FinishLaunchRequest { get; }
    }
}
