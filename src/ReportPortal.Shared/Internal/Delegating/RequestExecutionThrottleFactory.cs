using ReportPortal.Shared.Configuration;
using System;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <inheritdoc/>
    public class RequestExecutionThrottleFactory : IRequestExecutionThrottleFactory
    {
        private const int MAX_CONCURRENT_REQUESTS = 10;

        private IConfiguration _configuration;

        /// <summary>
        /// Initialize an instance with incoming configuration.
        /// </summary>
        /// <param name="configuration">Configuration for considering to create an instance.</param>
        public RequestExecutionThrottleFactory(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        public IRequestExecutionThrottler Create()
        {
            var maxConcurrentRequests = _configuration.GetValue("Server:MaximumConnectionsNumber", MAX_CONCURRENT_REQUESTS);

            return new RequestExecutionThrottler(maxConcurrentRequests);
        }
    }
}
