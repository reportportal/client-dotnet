using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    /// <summary>
    /// Represents the event arguments for the "BeforeTestStarting" event.
    /// </summary>
    public class BeforeTestStartingEventArgs : ReportEventBaseArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeTestStartingEventArgs"/> class.
        /// </summary>
        /// <param name="clientService">The client service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="startTestItemRequest">The start test item request.</param>
        public BeforeTestStartingEventArgs(IClientService clientService, IConfiguration configuration, StartTestItemRequest startTestItemRequest) : base(clientService, configuration)
        {
            StartTestItemRequest = startTestItemRequest;
        }

        /// <summary>
        /// Gets the start test item request.
        /// </summary>
        public StartTestItemRequest StartTestItemRequest { get; }
    }
}
