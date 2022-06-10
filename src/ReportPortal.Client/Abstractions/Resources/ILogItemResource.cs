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
        //Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request);

        Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request);

        /// <summary>
        /// Creates a new log item.
        /// </summary>
        /// <param name="request">Information about representation of log item.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Representation of just created log item.</returns>
        //Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request);

        Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new log item.
        /// </summary>
        /// <param name="requests">Information about representation of log item.</param>
        /// <returns>Representation of just created log item.</returns>
        //Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request);

        Task<LogItemsCreatedResponse> CreateAsync(params CreateLogItemRequest[] requests);

        /// <summary>
        /// Creates a new log item.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="requests">Information about representation of log item.</param>
        /// <returns>Representation of just created log item.</returns>
        //Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request);

        Task<LogItemsCreatedResponse> CreateAsync(CancellationToken cancellationToken, params CreateLogItemRequest[] requests);

        /// <summary>
        /// Deletes specified log item.
        /// </summary>
        /// <param name="id">ID of the log item to delete.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> DeleteAsync(long id);

        /// <summary>
        /// Deletes specified log item.
        /// </summary>
        /// <param name="id">ID of the log item to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns binary data of attached file to log message.
        /// </summary>
        /// <param name="id">ID of data.</param>
        /// <returns>Array of bytes.</returns>
        Task<byte[]> GetBinaryDataAsync(string id);

        /// <summary>
        /// Returns binary data of attached file to log message.
        /// </summary>
        /// <param name="id">ID of data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Array of bytes.</returns>
        Task<byte[]> GetBinaryDataAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns specified log item by ID.
        /// </summary>
        /// <param name="id">ID of the log item to retrieve.</param>
        /// <returns>A representation of log item/</returns>
        Task<LogItemResponse> GetAsync(long id);

        /// <summary>
        /// Returns specified log item by ID.
        /// </summary>
        /// <param name="id">ID of the log item to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representation of log item/</returns>
        Task<LogItemResponse> GetAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns specified log item by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the log item to retrieve.</param>
        /// <returns>A representation of log item/</returns>
        Task<LogItemResponse> GetAsync(string uuid);

        /// <summary>
        /// Returns specified log item by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the log item to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representation of log item/</returns>
        Task<LogItemResponse> GetAsync(string uuid, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a list of log items for specified test item.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving log items.</param>
        /// <returns>A list of log items.</returns>
        Task<Content<LogItemResponse>> GetAsync(FilterOption filterOption = null);

        /// <summary>
        /// Returns a list of log items for specified test item.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving log items.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of log items.</returns>
        Task<Content<LogItemResponse>> GetAsync(CancellationToken cancellationToken, FilterOption filterOption = null);
    }
}
