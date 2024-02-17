using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    /// <summary>
    /// Represents the event arguments for the BeforeTestFinishing event.
    /// </summary>
    public class BeforeTestFinishingEventArgs : ReportEventBaseArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeTestFinishingEventArgs"/> class.
        /// </summary>
        /// <param name="clientService">The client service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="finishTestItemRequest">The finish test item request.</param>
        public BeforeTestFinishingEventArgs(IClientService clientService, IConfiguration configuration, FinishTestItemRequest finishTestItemRequest) : base(clientService, configuration)
        {
            FinishTestItemRequest = finishTestItemRequest;
        }

        /// <summary>
        /// Gets the finish test item request.
        /// </summary>
        public FinishTestItemRequest FinishTestItemRequest { get; }
    }
}
