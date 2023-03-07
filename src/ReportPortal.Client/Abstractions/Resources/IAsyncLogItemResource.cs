using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    /// <summary>
    /// Asynchronously interacts with log items.
    /// </summary>
    public interface IAsyncLogItemResource
    {
        /// <summary>
        /// Asynchronously creates a new log item.
        /// </summary>
        /// <param name="request">Information about representation of log item.</param>
        /// <returns>Representation of just created log item.</returns>
        Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request);

        /// <inheritdoc cref="CreateAsync(CreateLogItemRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously creates a new log item.
        /// </summary>
        /// <param name="requests">Information about representation of log item.</param>
        /// <returns>Representation of just created log item.</returns>
        Task<LogItemsCreatedResponse> CreateAsync(CreateLogItemRequest[] requests);

        /// <inheritdoc cref="CreateAsync(CreateLogItemRequest[])"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LogItemsCreatedResponse> CreateAsync(CreateLogItemRequest[] requests, CancellationToken cancellationToken);
    }
}
