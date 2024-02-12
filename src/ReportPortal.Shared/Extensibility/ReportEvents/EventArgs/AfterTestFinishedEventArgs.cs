using ReportPortal.Client.Abstractions;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    public class AfterTestFinishedEventArgs : ReportEventBaseArgs
    {
        public AfterTestFinishedEventArgs(IClientService clientService, IConfiguration configuration) : base(clientService, configuration)
        {

        }
    }
}
