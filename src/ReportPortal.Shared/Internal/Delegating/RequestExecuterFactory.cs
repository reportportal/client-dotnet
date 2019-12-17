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

            var defaultStrategyValue = "exponential";

            var retryStrategy = configuration.GetValue("Server:Retry:Strategy", defaultStrategyValue) ?? defaultStrategyValue;

            IRequestExecuter executer;
            switch (retryStrategy.ToLowerInvariant())
            {
                case "exponential":
                    // TODO: configurable values
                    executer = new ExponentialRetryRequestExecuter(maxServiceConnections, 3, 2);
                    break;
                case "linear":
                    // TODO: configurable values
                    executer = new LinearRetryRequestExecuter(maxServiceConnections, 3, 5 * 1000);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Server:Retry:Strategy", $"Unknown '{retryStrategy}' retry strategy. Possible values are 'exponential' and 'linear'.");
            }

            return executer;
        }
    }
}
