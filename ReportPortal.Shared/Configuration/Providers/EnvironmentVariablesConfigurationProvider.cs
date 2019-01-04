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

        public EnvironmentVariablesConfigurationProvider(string prefix, string delimeter)
        {
            _prefix = prefix ?? string.Empty;
            _delimeter = delimeter ?? string.Empty;
        }

        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public IDictionary<string, string> Load()
        {
            Load(Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User));

            Load(Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process));

            return Properties;
        }

        void Load(IDictionary envVariables)
        {
            var variables = envVariables.Cast<DictionaryEntry>().Where(v => ((string)v.Key).StartsWith(_prefix, StringComparison.OrdinalIgnoreCase));

            foreach (var variable in variables)
            {
                Properties[((string)variable.Key).Substring(_prefix.Length + _delimeter.Length)] = (string)variable.Value;
            }
        }
    }
}
