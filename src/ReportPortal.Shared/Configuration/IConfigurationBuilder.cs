using System.Collections.Generic;

namespace ReportPortal.Shared.Configuration
{
    /// <summary>
    /// Builds <see cref="IConfiguration"/> instance to retrieve configuration variables from different providers.
    /// </summary>
    public interface IConfigurationBuilder
    {
        /// <summary>
        /// Gets registered providers.
        /// </summary>
        IList<IConfigurationProvider> Providers { get; }

        /// <summary>
        /// Register provider to be considered as configuration source.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns>The same <see cref="IConfigurationBuilder"/> instance.</returns>
        IConfigurationBuilder Add(IConfigurationProvider provider);

        /// <summary>
        /// Asks all registered providers to fetch configuration variables from a source.
        /// </summary>
        /// <returns>Configuration instance with fethed configuration variables.</returns>
        IConfiguration Build();
    }
}
