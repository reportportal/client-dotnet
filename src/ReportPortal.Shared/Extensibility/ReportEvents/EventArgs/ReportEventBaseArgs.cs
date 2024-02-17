using ReportPortal.Client.Abstractions;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    /// <summary>
    /// Base class for report event arguments.
    /// </summary>
    public abstract class ReportEventBaseArgs : System.EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportEventBaseArgs"/> class.
        /// </summary>
        /// <param name="clientService">The client service.</param>
        /// <param name="configuration">The configuration.</param>
        public ReportEventBaseArgs(IClientService clientService, IConfiguration configuration)
        {
            ClientService = clientService;
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the client service.
        /// </summary>
        public IClientService ClientService { get; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }
    }
}
