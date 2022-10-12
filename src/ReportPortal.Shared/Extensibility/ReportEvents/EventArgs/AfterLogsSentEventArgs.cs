using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using System.Collections.Generic;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    public class AfterLogsSentEventArgs : ReportEventBaseArgs
    {
        public AfterLogsSentEventArgs(IClientService clientService,
                                          IConfiguration configuration,
                                          IReadOnlyList<CreateLogItemRequest> createLogItemRequests) : base(clientService, configuration)
        {
            CreateLogItemRequests = createLogItemRequests;
        }

        public IReadOnlyList<CreateLogItemRequest> CreateLogItemRequests { get; }
    }
}
