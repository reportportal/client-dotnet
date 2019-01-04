using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

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
            var s = new DataContractJsonSerializer(typeof(Dictionary<string, object>), new DataContractJsonSerializerSettings() { UseSimpleDictionaryFormat = true });
            using (var file = File.OpenRead(_filePath))
            {
                // TODO: support deeper level deserialization
                var o = (Dictionary<string, object>)s.ReadObject(file);

                foreach (var property in o)
                {
                    Properties[property.Key] = property.Value.ToString();
                }

                return Properties;
            }
        }
    }
}
