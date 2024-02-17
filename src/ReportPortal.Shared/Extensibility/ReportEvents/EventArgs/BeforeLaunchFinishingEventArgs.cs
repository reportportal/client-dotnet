using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    /// <summary>
    /// Represents the event arguments for the BeforeLaunchFinishing event.
    /// </summary>
    public class BeforeLaunchFinishingEventArgs : ReportEventBaseArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeLaunchFinishingEventArgs"/> class.
        /// </summary>
        /// <param name="clientService">The client service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="finishLaunchRequest">The finish launch request.</param>
        public BeforeLaunchFinishingEventArgs(IClientService clientService, IConfiguration configuration, FinishLaunchRequest finishLaunchRequest) : base(clientService, configuration)
        {
            FinishLaunchRequest = finishLaunchRequest;
        }

        /// <summary>
        /// Gets the finish launch request.
        /// </summary>
        public FinishLaunchRequest FinishLaunchRequest { get; }
    }
}
