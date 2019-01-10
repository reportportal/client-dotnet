using System.Collections.Generic;

namespace ReportPortal.Shared.Configuration
{
    /// <summary>
    /// Provides a way to retrieve configuration variables.
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Returns fetched configuration variables.
        /// </summary>
        IDictionary<string, string> Properties { get; }

        /// <summary>
        /// Fethes configuration variables.
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> Load();
    }
}
