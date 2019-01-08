using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportPortal.Shared.Configuration
{
    public class Configuration : IConfiguration
    {
        public Configuration(IDictionary<string, object> values)
        {
            Values = values;
        }

        public IDictionary<string, object> Values { get; }

        public T GetValue<T>(string variable)
        {
            var propertyValue = Values[variable];

            return (T)Convert.ChangeType(propertyValue, typeof(T));
        }

        public T GetValue<T>(string variable, T defaultValue)
        {
            if (!Values.ContainsKey(variable))
            {
                return defaultValue;
            }
            else
            {
                return GetValue<T>(variable);
            }
        }

        public IEnumerable<T> GetValues<T>(string variable)
        {
            var propertyValue = Values[variable];

            var values = propertyValue.ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(i => (T)Convert.ChangeType(i, typeof(T))).ToList();

            return values;
        }

        public IEnumerable<T> GetValues<T>(string variable, IEnumerable<T> defaultValue)
        {
            if (!Values.ContainsKey(variable))
            {
                return defaultValue;
            }
            else
            {
                return GetValues<T>(variable);
            }
        }
    }
}
