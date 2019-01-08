using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ReportPortal.Shared.Configuration.Providers
{
    public class EnvironmentVariablesConfigurationProvider : IConfigurationProvider
    {
        private string _prefix;
        private string _delimeter;
        private EnvironmentVariableTarget _target;

        public EnvironmentVariablesConfigurationProvider(string prefix, string delimeter, EnvironmentVariableTarget target)
        {
            _prefix = prefix ?? string.Empty;
            _delimeter = delimeter ?? string.Empty;
            _target = target;
        }

        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public IDictionary<string, string> Load()
        {
            var variables = Environment.GetEnvironmentVariables(_target).Cast<DictionaryEntry>().Where(v => ((string)v.Key).StartsWith(_prefix, StringComparison.OrdinalIgnoreCase));

            foreach (var variable in variables)
            {
                Properties[((string)variable.Key).Substring(_prefix.Length + _delimeter.Length)] = (string)variable.Value;
            }

            return Properties;
        }
    }
}
