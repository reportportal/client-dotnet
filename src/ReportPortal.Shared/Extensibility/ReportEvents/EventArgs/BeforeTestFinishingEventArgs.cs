using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    public class BeforeTestFinishingEventArgs : ReportEventBaseArgs
    {
        public BeforeTestFinishingEventArgs(IClientService clientService, IConfiguration configuration, FinishTestItemRequest finishTestItemRequest) : base(clientService, configuration)
        {
            FinishTestItemRequest = finishTestItemRequest;
        }

        public FinishTestItemRequest FinishTestItemRequest { get; }
    }
}
