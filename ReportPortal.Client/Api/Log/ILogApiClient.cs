using System.Threading.Tasks;
using ReportPortal.Client.Api.Log.Model;
using ReportPortal.Client.Api.Log.Request;
using ReportPortal.Client.Common.Model;
using ReportPortal.Client.Common.Model.Filtering;
using ReportPortal.Client.Common.Model.Paging;

namespace ReportPortal.Client.Api.Log
{
    public interface ILogApiClient
    {
        /// <summary>
        /// Returns a list of log items for specified test item.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving log items.</param>
        /// <returns>A list of log items.</returns>
        Task<PagingContent<LogItem>> GetLogItemsAsync(FilterOption filterOption = null);

        /// <summary>
        /// Returns specified log item by ID.
        /// </summary>
        /// <param name="id">ID of the log item to retrieve.</param>
        /// <returns>A representation of log item/</returns>
        Task<LogItem> GetLogItemAsync(string id);

        /// <summary>
        /// Returns binary data of attached file to log message.
        /// </summary>
        /// <param name="id">ID of data.</param>
        /// <returns>Array of bytes.</returns>
        Task<byte[]> GetBinaryDataAsync(string id);

        /// <summary>
        /// Creates a new log item.
        /// </summary>
        /// <param name="model">Information about representation of log item.</param>
        /// <returns>Representation of just created log item.</returns>
        Task<LogItem> AddLogItemAsync(AddLogItemRequest model);

        /// <summary>
        /// Deletes specified log item.
        /// </summary>
        /// <param name="id">ID of the log item to delete.</param>
        /// <returns>A message from service.</returns>
        Task<Message> DeleteLogItemAsync(string id);
    }
}
