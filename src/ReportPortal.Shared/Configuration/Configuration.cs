using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportPortal.Shared.Configuration
{
    /// <inheritdoc />
    public class Configuration : IConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="Configuration"/> class and provide a way to retrieve properties.
        /// </summary>
        /// <param name="values"></param>
        public Configuration(IDictionary<string, object> values)
        {
            Properties = values;
        }

        private readonly string _notFoundMessage = "Property '{0}' not found in the configuration.";

        /// <inheritdoc />
        public IDictionary<string, object> Properties { get; }

        /// <inheritdoc />
        public T GetValue<T>(string property)
        {
            if (!Properties.ContainsKey(property))
            {
                throw new KeyNotFoundException(string.Format(_notFoundMessage, property));
            }

            var propertyValue = Properties[property];

            return (T)Convert.ChangeType(propertyValue, typeof(T));
        }

        /// <inheritdoc />
        public T GetValue<T>(string property, T defaultValue)
        {
            if (!Properties.ContainsKey(property))
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
            if (!Properties.ContainsKey(property))
            {
                throw new KeyNotFoundException(string.Format(_notFoundMessage, property));
            }

            var propertyValue = Properties[property];

            var values = propertyValue.ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(i => (T)Convert.ChangeType(i, typeof(T))).ToList();

            return values;
        }

        /// <inheritdoc />
        public IEnumerable<T> GetValues<T>(string property, IEnumerable<T> defaultValue)
        {
            if (!Properties.ContainsKey(property))
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
