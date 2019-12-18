using ReportPortal.Shared.Configuration;
using System;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <inheritdoc/>
    public class RequestExecuterFactory : IRequestExecuterFactory
    {
        private IConfiguration _configuration;

        /// <summary>
        /// Initializes new instance of <see cref="RequestExecuterFactory"/>
        /// </summary>
        /// <param name="configuration">Configuration object for considering when structs new <see cref="IRequestExecuter"/> instance.</param>
        public RequestExecuterFactory(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _configuration = configuration;
        }

        /// <inheritdoc/>
        public IRequestExecuter Create()
        {
            var throttler = new RequestExecutionThrottleFactory(_configuration).Create();

            var defaultStrategyValue = "exponential";

            var retryStrategy = _configuration.GetValue("Server:Retry:Strategy", defaultStrategyValue) ?? defaultStrategyValue;

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
