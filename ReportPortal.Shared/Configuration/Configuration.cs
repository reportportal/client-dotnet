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
            return (T)Values[variable];
        }
    }
}
