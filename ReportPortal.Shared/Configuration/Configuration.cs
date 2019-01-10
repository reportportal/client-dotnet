using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportPortal.Shared.Configuration
{
    /// <inheritdoc />
    public class Configuration : IConfiguration
    {
        public Configuration(IDictionary<string, object> values)
        {
            Values = values;
        }

        /// <inheritdoc />
        public IDictionary<string, object> Values { get; }

        /// <inheritdoc />
        public T GetValue<T>(string property)
        {
            var propertyValue = Values[property];

            return (T)Convert.ChangeType(propertyValue, typeof(T));
        }

        /// <inheritdoc />
        public T GetValue<T>(string property, T defaultValue)
        {
            if (!Values.ContainsKey(property))
            {
                return defaultValue;
            }
            else
            {
                return GetValue<T>(property);
            }
        }

        /// <inheritdoc />
        public IEnumerable<T> GetValues<T>(string property)
        {
            var propertyValue = Values[property];

            var values = propertyValue.ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(i => (T)Convert.ChangeType(i, typeof(T))).ToList();

            return values;
        }

        /// <inheritdoc />
        public IEnumerable<T> GetValues<T>(string property, IEnumerable<T> defaultValue)
        {
            if (!Values.ContainsKey(property))
            {
                return defaultValue;
            }
            else
            {
                return GetValues<T>(property);
            }
        }
    }
}
