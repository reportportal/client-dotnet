using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace ReportPortal.Shared.Configuration.Providers
{
    /// <summary>
    /// Parse json file with configuration properties as flatten dictionary.
    /// </summary>
    public class JsonFileConfigurationProvider : IConfigurationProvider
    {
        static JsonFileConfigurationProvider()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("System.Text.Json", StringComparison.OrdinalIgnoreCase))
            {
                return Assembly.Load("System.Text.Json");
            }

            return null;
        }

        private readonly string _delimeter;
        private readonly string _filePath;
        private readonly bool _optional;

        /// <summary>
        /// Creates new instance of <see cref="JsonFileConfigurationProvider" /> class.
        /// </summary>
        /// <param name="delimeter">Char which represents hierarchy of flatten properties.</param>
        /// <param name="filePath">The path to json file.</param>
        /// <param name="optional">If file doesn't exist then empty disctionary will be returns.</param>
        public JsonFileConfigurationProvider(string delimeter, string filePath, bool optional)
        {
            _delimeter = delimeter;
            _filePath = filePath;
            _optional = optional;
        }

        /// <inheritdoc />
        public IDictionary<string, string> Load()
        {
            var properties = new Dictionary<string, string>();

            var directory = Path.GetDirectoryName(_filePath);

            if (string.IsNullOrEmpty(directory)) directory = Directory.GetCurrentDirectory();

            var files = Directory.GetFiles(directory);

            var filePath = files.FirstOrDefault(f => f.ToUpperInvariant() == _filePath.ToUpperInvariant());

            if (filePath != null)
            {
                var json = File.ReadAllText(filePath);

                var flattenProperties = GetFlattenProperties(json);

                foreach (var property in flattenProperties)
                {
                    properties[property.Key] = property.Value;
                }
            }
            else if (!_optional)
            {
                throw new FileLoadException($"Required configuration file '{_filePath}' was not found.");
            }

            return properties;
        }

        private Dictionary<string, string> GetFlattenProperties(string json)
        {
            var properties = new Dictionary<string, string>();

            using (var jsonDocument = JsonDocument.Parse(json))
            {
                foreach (var jsonProperty in jsonDocument.RootElement.EnumerateObject())
                {
                    foreach (var item in ParseJsonProperty(jsonProperty))
                    {
                        properties.Add(item.Key, item.Value);
                    }
                }
            }

            return properties;
        }

        private Dictionary<string, string> ParseJsonProperty(JsonProperty jsonProperty, string parentPropertyName = null)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            var propertyName = jsonProperty.Name;

            if (parentPropertyName != null)
            {
                propertyName = $"{parentPropertyName}{_delimeter}{propertyName}";
            }

            if (jsonProperty.Value.ValueKind == JsonValueKind.Object)
            {
                foreach (var innerJsonProperty in jsonProperty.Value.EnumerateObject())
                {
                    foreach (var item in ParseJsonProperty(innerJsonProperty, propertyName))
                    {
                        properties.Add(item.Key, item.Value);
                    }
                }
            }
            else if (jsonProperty.Value.ValueKind == JsonValueKind.Array)
            {
                var propertyValue = "";

                foreach (var item in jsonProperty.Value.EnumerateArray())
                {
                    propertyValue += $"{item};";
                }

                properties.Add(propertyName, propertyValue);
            }
            else
            {
                properties.Add(propertyName, jsonProperty.Value.ToString());
            }

            return properties;
        }
    }
}
