using ReportPortal.Client.Abstractions;
using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Extensibility.ReportEvents.EventArgs
{
    public abstract class ReportEventBaseArgs : System.EventArgs
    {
        public ReportEventBaseArgs(IClientService clientService, IConfiguration configuration)
        {
            ClientService = clientService;
            Configuration = configuration;
        }

        public IClientService ClientService { get; }

        public IConfiguration Configuration { get; }
    }
}
