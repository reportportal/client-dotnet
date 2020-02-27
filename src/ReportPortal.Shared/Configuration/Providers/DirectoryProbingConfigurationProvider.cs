using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReportPortal.Shared.Configuration.Providers
{
    /// <summary>
    /// Finds files in a directory and consider their content as a value for configuration properties.
    /// </summary>
    public class DirectoryProbingConfigurationProvider : IConfigurationProvider
    {
        private readonly string _directoryPath;
        private readonly string _prefix;
        private readonly string _delimeter;
        private readonly bool _optional;

        /// <summary>
        /// Creates new instance of <see cref="DirectoryProbingConfigurationProvider"/> class.
        /// </summary>
        /// <param name="directoryPath">Path to a directory where to find files.</param>
        /// <param name="prefix">Limit files searching.</param>
        /// <param name="delimeter">Consider this string as hierarchic property.</param>
        /// <param name="optional">Returns empty list of properties if directory doesn't exist.</param>
        public DirectoryProbingConfigurationProvider(string directoryPath, string prefix, string delimeter, bool optional)
        {
            _directoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
            _prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
            _delimeter = delimeter ?? throw new ArgumentNullException(nameof(delimeter));
            _optional = optional;
        }

        /// <inheritdoc />
        public IDictionary<string, string> Load()
        {
            var properties = new Dictionary<string, string>();

            if (Directory.Exists(_directoryPath))
            {
                var directory = new DirectoryInfo(_directoryPath);

                var escapedDelimeter = Regex.Escape(_delimeter);
                var pattern = $"{_prefix.ToLowerInvariant()}({escapedDelimeter}[a-zA-Z]+)+";

                var ignoredFileExtensions = new string[] { ".exe", ".dll", ".log" };

                var candidates = directory.EnumerateFiles().Where(f => Regex.IsMatch(f.Name.ToLowerInvariant(), pattern) && !ignoredFileExtensions.Contains(f.Extension.ToLowerInvariant()));

                foreach (var candidate in candidates)
                {
                    var key = candidate.Name.ToLowerInvariant().Replace($"{_prefix.ToLowerInvariant()}{_delimeter}", string.Empty).Replace(_delimeter, ConfigurationPath.KeyDelimeter);
                    var value = File.ReadAllText(candidate.FullName);

                    properties[key] = value;
                }
            }
            else
            {
                if (!_optional)
                {
                    throw new DirectoryNotFoundException($"Required directory not found by '{_directoryPath}' path as configuration provider.");
                }
            }

            return properties;
        }
    }
}
