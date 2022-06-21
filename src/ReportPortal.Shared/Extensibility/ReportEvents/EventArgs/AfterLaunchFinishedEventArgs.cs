using ReportPortal.Client.Abstractions;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    public class AfterLaunchFinishedEventArgs : ReportEventBaseArgs
    {
        public AfterLaunchFinishedEventArgs(IClientService clientService, IConfiguration configuration) : base(clientService, configuration)
        {

        }
    }
}
