using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace ReportPortal.Shared.Configuration.Providers
{
    public class JsonFileConfigurationProvider : IConfigurationProvider
    {
        private string _filePath;

        public JsonFileConfigurationProvider(string filePath)
        {
            _filePath = filePath;
        }

        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public IDictionary<string, string> Load()
        {
            var json = File.ReadAllText(_filePath);

            var properties = GetFlattenProperties(json);

            foreach (var property in properties)
            {
                Properties[property.Key] = property.Value;
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
                    propertyName += $"__{jsonReader.Name}";
                }
                else if (jsonReader.NodeType == XmlNodeType.EndElement)
                {
                    if (jsonReader.Name != "item" && jsonReader.Name != "root" && propertyValue != null)
                    {
                        properties[propertyName.Replace("__root__", "")] = propertyValue;

                        propertyValue = null;
                    }

                    propertyName = propertyName.Substring(0, propertyName.Length - jsonReader.Name.Length - 2);
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
