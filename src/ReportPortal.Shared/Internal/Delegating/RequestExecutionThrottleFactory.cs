using ReportPortal.Shared.Configuration;
using System;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <inheritdoc/>
    public class RequestExecutionThrottleFactory : IRequestExecutionThrottleFactory
    {
        private IConfiguration _configuration;

        /// <summary>
        /// Initialize an instance with incoming configuration.
        /// </summary>
        /// <param name="configuration">Configuration for considering to cerate an instance.</param>
        public RequestExecutionThrottleFactory(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _configuration = configuration;
        }

        /// <inheritdoc/>
        public IRequestExecutionThrottler Create()
        {
            var maxServiceConnections = _configuration.GetValue("Server:MaximumConnectionsNumber", int.MaxValue);

            return new RequestExecutionThrottler(maxServiceConnections);
        }
    }
}
