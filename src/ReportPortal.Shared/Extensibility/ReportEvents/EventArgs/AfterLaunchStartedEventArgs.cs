using ReportPortal.Client.Abstractions;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    public class AfterLaunchStartedEventArgs : ReportEventBaseArgs
    {
        public AfterLaunchStartedEventArgs(IClientService clientService, IConfiguration configuration) : base(clientService, configuration)
        {

        }
    }
}
