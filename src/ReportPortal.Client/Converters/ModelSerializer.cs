using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Converters
{
    public class ModelSerializer
    {
        public static T Deserialize<T>(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(json, options);
        }

        public static T Deserialize<T>(Stream stream)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(stream, options);
        }

        public static string Serialize<T>(object obj)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(obj, options);
        }

        public static void Serialize<T>(object obj, Stream stream)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            JsonSerializer.Serialize(stream, obj, options);
        }
    }
}
