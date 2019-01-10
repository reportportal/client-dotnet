using System.Collections.Generic;

namespace ReportPortal.Shared.Configuration
{
    /// <summary>
    /// Stores configuration variables from different providers.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Fethed configuration variables.
        /// </summary>
        IDictionary<string, object> Values { get; }

        /// <summary>
        /// Returns value of configuration property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        T GetValue<T>(string property);

        T GetValue<T>(string property, T defaultValue);

        IEnumerable<T> GetValues<T>(string property);

        IEnumerable<T> GetValues<T>(string property, IEnumerable<T> defaultValue);
    }
}
