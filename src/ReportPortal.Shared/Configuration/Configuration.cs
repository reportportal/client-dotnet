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

        private readonly string _notFoundMessage = "Property '{0}' not found in the configuration. Make sure you have configured it properly.";

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

            return ConvertValue<T>(propertyValue);
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

            var values = propertyValue.ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            return values.Select(v => ConvertValue<T>(v)).ToList();
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
            foreach (var value in values)
            {
                var entries = value.Split(':');

                string key;
                string keyValue;

                if (entries.Length == 1)
                {
                    key = string.Empty;
                    keyValue = value;
                }
                else
                {
                    key = entries[0];
                    keyValue = value.Substring(key.Length + 1);
                }

                result.Add(new KeyValuePair<string, T>(key.Trim(), ConvertValue<T>(keyValue.Trim())));
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

        private T ConvertValue<T>(object value)
        {
            if (typeof(T) == typeof(bool))
            {
                var trueValues = new List<string> { "true", "yes", "y", "1" };
                var falseValues = new List<string> { "false", "no", "n", "0" };

                if (trueValues.Any(v => value.ToString().ToLowerInvariant() == v))
                {
                    return (T)(object)true;
                }
                else if (falseValues.Any(v => value.ToString().ToLowerInvariant() == v))
                {
                    return (T)(object)false;
                }
                else
                {
                    throw new InvalidCastException($"Unknown '{value}' value for '{typeof(T)}'.");
                }
            }
            else if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), value.ToString(), ignoreCase: true);
            }
            else
            {
                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
        }
    }
}
