using ReportPortal.Shared.Configuration;
using System;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <inheritdoc/>
    public class RequestExecuterFactory : IRequestExecuterFactory
    {
        /// <inheritdoc/>
        public IRequestExecuter Create(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var maxServiceConnections = configuration.GetValue("Server:MaximumConnectionsNumber", int.MaxValue);

            return new ExponentialRetryRequestExecuter(maxServiceConnections, 3, 2);
        }
    }
}
