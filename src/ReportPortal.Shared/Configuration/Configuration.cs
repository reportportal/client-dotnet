using System;
using System.Collections.Generic;
using System.Globalization;
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
                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, _notFoundMessage, property));
            }

            var propertyValue = Properties[property];

            return (T)Convert.ChangeType(propertyValue, typeof(T), CultureInfo.InvariantCulture);
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
                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, _notFoundMessage, property));
            }

            var propertyValue = Properties[property];

            var values = propertyValue.ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(i => (T)Convert.ChangeType(i, typeof(T), CultureInfo.InvariantCulture)).ToList();

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

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, T>> GetKeyValues<T>(string property)
        {
            if (!Properties.ContainsKey(property))
            {
                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, _notFoundMessage, property));
            }

            var result = new List<KeyValuePair<string, T>>();

            var values = Properties[property].ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var value in values)
            {
                var entries = value.Split(':');

                string key;
                string keyValue;

                if(entries.Length == 1)
                {
                    key = string.Empty;
                    keyValue = value;
                }
                else
                {
                    key = entries[0];
                    keyValue = value.Substring(key.Length + 1);
                }

                result.Add(new KeyValuePair<string, T>(key, (T)Convert.ChangeType(keyValue, typeof(T), CultureInfo.InvariantCulture)));
            }

            return result;
        }

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, T>> GetKeyValues<T>(string property, IEnumerable<KeyValuePair<string, T>> defaultValue)
        {
            if (!Properties.ContainsKey(property))
            {
                return defaultValue;
            }
            else
            {
                return GetKeyValues<T>(property);
            }
        }
    }
}
