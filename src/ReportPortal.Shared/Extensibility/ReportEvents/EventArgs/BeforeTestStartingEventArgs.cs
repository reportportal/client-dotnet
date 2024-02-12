using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    public class BeforeTestStartingEventArgs : ReportEventBaseArgs
    {
        public BeforeTestStartingEventArgs(IClientService clientService, IConfiguration configuration, StartTestItemRequest startTestItemRequest) : base(clientService, configuration)
        {
            StartTestItemRequest = startTestItemRequest;
        }

        public StartTestItemRequest StartTestItemRequest { get; }
    }
}
