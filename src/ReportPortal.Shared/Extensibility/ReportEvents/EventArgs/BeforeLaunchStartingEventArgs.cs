using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    /// <summary>
    /// Represents the event arguments for the BeforeLaunchStarting event.
    /// </summary>
    public class BeforeLaunchStartingEventArgs : ReportEventBaseArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeLaunchStartingEventArgs"/> class.
        /// </summary>
        /// <param name="clientService">The client service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="startLaunchRequest">The start launch request.</param>
        public BeforeLaunchStartingEventArgs(IClientService clientService, IConfiguration configuration, StartLaunchRequest startLaunchRequest) : base(clientService, configuration)
        {
            StartLaunchRequest = startLaunchRequest;
        }

        /// <summary>
        /// Gets the start launch request.
        /// </summary>
        public StartLaunchRequest StartLaunchRequest { get; }
    }
}
