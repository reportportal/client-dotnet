using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <inheritdoc/>
    public class RequestExecuterFactory : IRequestExecuterFactory
    {
        /// <inheritdoc/>
        public IRequestExecuter Create(IConfiguration configuration)
        {
            var maxServiceConnections = configuration.GetValue("Server:MaximumConnectionsNumber", int.MaxValue);

            return new ExponentialRetryRequestExecuter(maxServiceConnections, 3, 2);
        }
    }
}
