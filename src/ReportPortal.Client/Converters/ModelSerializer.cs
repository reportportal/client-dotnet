using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Converters
{
    internal class ModelSerializer
    {
        /// <summary>
        /// Deserializes the content of a stream into an object of type T asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="stream">The stream containing the serialized content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static async Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            return (T)await JsonSerializer.DeserializeAsync(stream, typeof(T), ClientSourceGenerationContext.Default, cancellationToken);
        }

        /// <summary>
        /// Serializes an object into a stream asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="stream">The stream to write the serialized content to.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SerializeAsync<T>(object obj, Stream stream, CancellationToken cancellationToken = default)
        {
            await JsonSerializer.SerializeAsync(stream, obj, typeof(T), ClientSourceGenerationContext.Default, cancellationToken);
        }
    }
}