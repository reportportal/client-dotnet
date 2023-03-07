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
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Representation of just created log item.</returns>
        Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously creates a new log item.
        /// </summary>
        /// <param name="requests">Information about representation of log item.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Representation of just created log item.</returns>
        Task<LogItemsCreatedResponse> CreateAsync(CreateLogItemRequest[] requests, CancellationToken cancellationToken = default);
    }
}
