using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using System.Collections.Generic;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    /// <summary>
    /// Represents the event arguments for the BeforeLogsSending event.
    /// </summary>
    public class BeforeLogsSendingEventArgs : ReportEventBaseArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeLogsSendingEventArgs"/> class.
        /// </summary>
        /// <param name="clientService">The client service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="createLogItemRequests">The list of create log item requests.</param>
        public BeforeLogsSendingEventArgs(IClientService clientService,
                                          IConfiguration configuration,
                                          IList<CreateLogItemRequest> createLogItemRequests) : base(clientService, configuration)
        {
            CreateLogItemRequests = createLogItemRequests;
        }

        /// <summary>
        /// Gets the list of create log item requests.
        /// </summary>
        public IList<CreateLogItemRequest> CreateLogItemRequests { get; }
    }
}
