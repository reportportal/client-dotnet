using ReportPortal.Client.Abstractions;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    /// <summary>
    /// Represents the event arguments for the AfterLaunchStarted event.
    /// </summary>
    public class AfterLaunchStartedEventArgs : ReportEventBaseArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AfterLaunchStartedEventArgs"/> class.
        /// </summary>
        /// <param name="clientService">The client service.</param>
        /// <param name="configuration">The configuration.</param>
        public AfterLaunchStartedEventArgs(IClientService clientService, IConfiguration configuration) : base(clientService, configuration)
        {

        }
    }
}
