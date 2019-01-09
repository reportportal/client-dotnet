using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace ReportPortal.Shared.Configuration.Providers
{
    public class JsonFileConfigurationProvider : IConfigurationProvider
    {
        private string _delimeter;
        private string _filePath;
        private bool _optional;

        public JsonFileConfigurationProvider(string delimeter, string filePath, bool optional)
        {
            _delimeter = delimeter;
            _filePath = filePath;
            _optional = optional;
        }

        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public IDictionary<string, string> Load()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);

                var properties = GetFlattenProperties(json);

                foreach (var property in properties)
                {
                    Properties[property.Key] = property.Value;
                }
            }
            else if (!_optional)
            {
                throw new FileLoadException($"Required configuration file '{_filePath}' was not found.");
            }

            return Properties;
        }

        private Dictionary<string, string> GetFlattenProperties(string json)
        {
            var properties = new Dictionary<string, string>();

            var jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), new XmlDictionaryReaderQuotas());

            string propertyName = string.Empty;
            string propertyValue = null;

            while (jsonReader.Read())
            {
                if (jsonReader.NodeType == XmlNodeType.Element)
                {
                    propertyName += $"{_delimeter}{jsonReader.Name}";
                }
                else if (jsonReader.NodeType == XmlNodeType.EndElement)
                {
                    if (jsonReader.Name != "item" && jsonReader.Name != "root" && propertyValue != null)
                    {
                        properties[propertyName.Replace($"{_delimeter}root{_delimeter}", "").Replace(_delimeter, ConfigurationPath.KEYDELIMETER)] = propertyValue;

                        propertyValue = null;
                    }

                    propertyName = propertyName.Substring(0, propertyName.Length - jsonReader.Name.Length - _delimeter.Length);
                }
                else if (jsonReader.NodeType == XmlNodeType.Text)
                {
                    if (propertyName.EndsWith("item"))
                    {
                        propertyValue += $"{jsonReader.Value};";
                    }
                    else
                    {
                        propertyValue = jsonReader.Value;
                    }
                }
            }

            return properties;
        }
    }
}
