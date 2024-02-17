using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    /// <summary>
    /// Interacts with log items.
    /// </summary>
    public interface ILogItemResource
    {
        /// <summary>
        /// Creates a new log item.
        /// </summary>
        /// <param name="request">Information about representation of log item.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Representation of just created log item.</returns>
        Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new log item.
        /// </summary>
        /// <param name="requests">Information about representation of log item.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Representation of just created log item.</returns>
        Task<LogItemsCreatedResponse> CreateAsync(CreateLogItemRequest[] requests, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes specified log item.
        /// </summary>
        /// <param name="id">ID of the log item to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns binary data of attached file to log message.
        /// </summary>
        /// <param name="id">ID of data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Array of bytes.</returns>
        Task<byte[]> GetBinaryDataAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns specified log item by ID.
        /// </summary>
        /// <param name="id">ID of the log item to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representation of log item/</returns>
        Task<LogItemResponse> GetAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns specified log item by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the log item to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representation of log item/</returns>
        Task<LogItemResponse> GetAsync(string uuid, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a list of log items for specified test item.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of log items.</returns>
        Task<Content<LogItemResponse>> GetAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a list of log items for specified test item.
        /// </summary>
        /// <param name="filterOption">Specified criteria for retrieving log items.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of log items.</returns>
        Task<Content<LogItemResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken = default);
    }
}
