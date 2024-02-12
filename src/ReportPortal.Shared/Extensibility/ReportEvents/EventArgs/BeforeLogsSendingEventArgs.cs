using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using System.Collections.Generic;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    public class BeforeLogsSendingEventArgs : ReportEventBaseArgs
    {
        public BeforeLogsSendingEventArgs(IClientService clientService,
                                          IConfiguration configuration,
                                          IList<CreateLogItemRequest> createLogItemRequests) : base(clientService, configuration)
        {
            CreateLogItemRequests = createLogItemRequests;
        }

        public IList<CreateLogItemRequest> CreateLogItemRequests { get; }
    }
}
