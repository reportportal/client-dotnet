using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReportPortal.Shared.Configuration.Providers
{
    public class DirectoryProbingConfigurationProvider : IConfigurationProvider
    {
        private string _directoryPath;
        private string _prefix;
        private string _delimeter;
        private bool _optional;

        public DirectoryProbingConfigurationProvider(string directoryPath, string prefix, string delimeter, bool optional)
        {
            if (directoryPath == null) throw new ArgumentNullException(nameof(directoryPath));
            if (prefix == null) throw new ArgumentNullException(nameof(prefix));
            if (delimeter == null) throw new ArgumentNullException(nameof(delimeter));

            _directoryPath = directoryPath;
            _prefix = prefix;
            _delimeter = delimeter;
            _optional = optional;
        }

        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public IDictionary<string, string> Load()
        {
            if (Directory.Exists(_directoryPath))
            {
                var directory = new DirectoryInfo(_directoryPath);

                var escapedDelimeter = Regex.Escape(_delimeter);
                var pattern = $"{_prefix.ToLowerInvariant()}({escapedDelimeter}[a-zA-Z]+)+";

                var candidates = directory.EnumerateFiles().Where(f => Regex.IsMatch(f.Name.ToLowerInvariant(), pattern) && !f.Extension.Equals(".dll", StringComparison.OrdinalIgnoreCase) && !f.Extension.Equals(".exe", StringComparison.OrdinalIgnoreCase));

                foreach (var candidate in candidates)
                {
                    var key = candidate.Name.ToLowerInvariant().Replace($"{_prefix}{_delimeter}", string.Empty).Replace(_delimeter, ConfigurationPath.KeyDelimeter);
                    var value = File.ReadAllText(candidate.FullName);

                    Properties[key] = value;
                }
            }
            else
            {
                if (!_optional)
                {
                    throw new DirectoryNotFoundException($"Required directory not found by '{_directoryPath}' path as configuration provider.");
                }
            }

            return Properties;
        }
    }
}
