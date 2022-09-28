using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Converters
{
    internal class ModelSerializer
    {
        public static async Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            return (T)await JsonSerializer.DeserializeAsync(stream, typeof(T), ClientSourceGenerationContext.Default, cancellationToken);
        }

        public static async Task SerializeAsync<T>(object obj, Stream stream, CancellationToken cancellationToken = default)
        {
            await JsonSerializer.SerializeAsync(stream, obj, typeof(T), ClientSourceGenerationContext.Default, cancellationToken);
        }
    }
}
