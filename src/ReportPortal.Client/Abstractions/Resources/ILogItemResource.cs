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
        /// <returns>Representation of just created log item.</returns>
        Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request);

        /// <inheritdoc cref="CreateAsync(CreateLogItemRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new log item.
        /// </summary>
        /// <param name="requests">Information about representation of log item.</param>
        /// <returns>Representation of just created log item.</returns>
        Task<LogItemsCreatedResponse> CreateAsync(CreateLogItemRequest[] requests);

        /// <inheritdoc cref="CreateAsync(CreateLogItemRequest[])"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LogItemsCreatedResponse> CreateAsync(CreateLogItemRequest[] requests, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes specified log item.
        /// </summary>
        /// <param name="id">ID of the log item to delete.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> DeleteAsync(long id);

        /// <inheritdoc cref="DeleteAsync(long)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns binary data of attached file to log message.
        /// </summary>
        /// <param name="id">ID of data.</param>
        /// <returns>Array of bytes.</returns>
        Task<byte[]> GetBinaryDataAsync(string id);

        /// <inheritdoc cref="GetBinaryDataAsync(string)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<byte[]> GetBinaryDataAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns specified log item by ID.
        /// </summary>
        /// <param name="id">ID of the log item to retrieve.</param>
        /// <returns>A representation of log item/</returns>
        Task<LogItemResponse> GetAsync(long id);

        /// <inheritdoc cref="GetAsync(long)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LogItemResponse> GetAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns specified log item by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the log item to retrieve.</param>
        /// <returns>A representation of log item/</returns>
        Task<LogItemResponse> GetAsync(string uuid);

        /// <inheritdoc cref="GetAsync(string)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LogItemResponse> GetAsync(string uuid, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a list of log items for specified test item.
        /// </summary>

        /// <returns>A list of log items.</returns>
        Task<Content<LogItemResponse>> GetAsync();

        /// <inheritdoc cref="GetAsync()"/>
        /// <param name="filterOption">Specified criterias for retrieving log items.</param>
        Task<Content<LogItemResponse>> GetAsync(FilterOption filterOption);

        /// <inheritdoc cref="GetAsync()"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<Content<LogItemResponse>> GetAsync(CancellationToken cancellationToken);

        /// <inheritdoc cref="GetAsync(FilterOption)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<Content<LogItemResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken);
    }
}
