using ReportPortal.Client.Abstractions;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    /// <summary>
    /// Represents the event arguments for the AfterLaunchFinished event.
    /// </summary>
    public class AfterLaunchFinishedEventArgs : ReportEventBaseArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AfterLaunchFinishedEventArgs"/> class.
        /// </summary>
        /// <param name="clientService">The client service.</param>
        /// <param name="configuration">The configuration.</param>
        public AfterLaunchFinishedEventArgs(IClientService clientService, IConfiguration configuration) : base(clientService, configuration)
        {

        }
    }
}
