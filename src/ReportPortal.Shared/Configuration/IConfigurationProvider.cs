using System.Collections.Generic;

namespace ReportPortal.Shared.Configuration
{
    /// <summary>
    /// Provides a way to retrieve configuration variables.
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Fethes configuration variables as dictionary.
        /// </summary>
        /// <returns>Dictionary where key is property name and value is property value.</returns>
        IDictionary<string, string> Load();
    }
}
