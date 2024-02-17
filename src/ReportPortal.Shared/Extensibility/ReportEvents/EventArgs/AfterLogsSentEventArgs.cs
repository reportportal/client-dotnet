using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using System.Collections.Generic;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    /// <summary>
    /// Represents the event arguments for the AfterLogsSent event.
    /// </summary>
    public class AfterLogsSentEventArgs : ReportEventBaseArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AfterLogsSentEventArgs"/> class.
        /// </summary>
        /// <param name="clientService">The client service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="createLogItemRequests">The list of log item requests.</param>
        public AfterLogsSentEventArgs(IClientService clientService,
                                      IConfiguration configuration,
                                      IReadOnlyList<CreateLogItemRequest> createLogItemRequests) : base(clientService, configuration)
        {
            CreateLogItemRequests = createLogItemRequests;
        }

        /// <summary>
        /// Gets the list of log item requests.
        /// </summary>
        public IReadOnlyList<CreateLogItemRequest> CreateLogItemRequests { get; }
    }
}
