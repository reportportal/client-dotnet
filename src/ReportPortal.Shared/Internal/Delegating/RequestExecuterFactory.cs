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

            IRequestExecutionThrottler throttler = null;
            if (configuration.Values != null && configuration.Values.ContainsKey("Server:MaximumConnectionsNumber"))
            {
                var maxServiceConnections = configuration.GetValue<int>("Server:MaximumConnectionsNumber");

                throttler = new RequestExecutionThrottler(maxServiceConnections);
            }

            var defaultStrategyValue = "exponential";

            var retryStrategy = configuration.GetValue("Server:Retry:Strategy", defaultStrategyValue) ?? defaultStrategyValue;

            IRequestExecuter executer;
            switch (retryStrategy.ToLowerInvariant())
            {
                case "exponential":
                    // TODO: configurable values
                    executer = new ExponentialRetryRequestExecuter(3, 2, throttler);
                    break;
                case "linear":
                    // TODO: configurable values
                    executer = new LinearRetryRequestExecuter(3, 5 * 1000, throttler);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Server:Retry:Strategy", $"Unknown '{retryStrategy}' retry strategy. Possible values are 'exponential' and 'linear'.");
            }

            return executer;
        }
    }
}
