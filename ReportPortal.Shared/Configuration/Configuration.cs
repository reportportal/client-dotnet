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

        private readonly string _notFoundMessage = "Property '{0}' not found in the configuration.";

        /// <inheritdoc />
        public IDictionary<string, object> Values { get; }

        /// <inheritdoc />
        public T GetValue<T>(string property)
        {
            if (!Values.ContainsKey(property))
            {
                throw new KeyNotFoundException(string.Format(_notFoundMessage, property));
            }

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
            if (!Values.ContainsKey(property))
            {
                throw new KeyNotFoundException(string.Format(_notFoundMessage, property));
            }

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
