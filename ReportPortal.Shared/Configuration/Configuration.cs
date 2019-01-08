using System;
using System.Collections.Generic;
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
    }
}
