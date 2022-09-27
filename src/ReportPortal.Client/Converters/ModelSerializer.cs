using System.IO;
using System.Text.Json;

namespace ReportPortal.Client.Converters
{
    public class ModelSerializer
    {
        public static T Deserialize<T>(string json)
        {
            return (T)JsonSerializer.Deserialize(json, typeof(T), ClientSourceGenerationContext.Default);
        }

        public static T Deserialize<T>(Stream stream)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return (T)JsonSerializer.Deserialize(stream, typeof(T), ClientSourceGenerationContext.Default);
        }

        public static string Serialize<T>(object obj)
        {
            return JsonSerializer.Serialize(obj, typeof(T), ClientSourceGenerationContext.Default);
        }

        public static void Serialize<T>(object obj, Stream stream)
        {
            JsonSerializer.Serialize(stream, obj, typeof(T), ClientSourceGenerationContext.Default);
        }
    }
}
