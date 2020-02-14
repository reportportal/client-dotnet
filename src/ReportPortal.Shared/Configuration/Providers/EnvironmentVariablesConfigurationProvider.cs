using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ReportPortal.Shared.Configuration.Providers
{
    /// <summary>
    /// Retrieves environment variables as configuration properties.
    /// </summary>
    public class EnvironmentVariablesConfigurationProvider : IConfigurationProvider
    {
        private readonly string _prefix;
        private readonly string _delimeter;
        private readonly EnvironmentVariableTarget _target;

        /// <summary>
        /// Creates new instance of <see cref="EnvironmentVariablesConfigurationProvider"/> class.
        /// </summary>
        /// <param name="prefix">Only use environment variables which starts from spicific prefix.</param>
        /// <param name="delimeter">Property is considered as hierarchical if its name contains specific character.</param>
        /// <param name="target">Environment variables scope, like machine scoped or process scoped.</param>
        public EnvironmentVariablesConfigurationProvider(string prefix, string delimeter, EnvironmentVariableTarget target)
        {
            _prefix = prefix ?? string.Empty;
            _delimeter = delimeter ?? string.Empty;
            _target = target;
        }

        /// <inheritdoc />
        public IDictionary<string, string> Load()
        {
            var properties = new Dictionary<string, string>();

            var variables = Environment.GetEnvironmentVariables(_target).Cast<DictionaryEntry>().Where(v => ((string)v.Key).StartsWith(_prefix, StringComparison.OrdinalIgnoreCase));

            foreach (var variable in variables)
            {
                properties[((string)variable.Key).Substring(_prefix.Length).Replace(_delimeter, ConfigurationPath.KeyDelimeter)] = (string)variable.Value;
            }

            return properties;
        }
    }
}
