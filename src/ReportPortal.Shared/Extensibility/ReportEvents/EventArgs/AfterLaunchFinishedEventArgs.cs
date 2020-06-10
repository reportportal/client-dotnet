using ReportPortal.Client.Abstractions;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Reporter;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    public class AfterLaunchFinishedEventArgs : ReportEventBaseArgs
    {
        public AfterLaunchFinishedEventArgs(IClientService clientService, IConfiguration configuration) : base(clientService, configuration)
        {

        }
    }
}
