using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Models;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions
{
    public interface ILogItemResource
    {
        Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest model);
        Task<Message> DeleteAsync(long id);
        Task<byte[]> GetBinaryDataAsync(string id);
        Task<LogItem> GetAsync(long id);
        Task<LogItem> GetAsync(string uuid);
        Task<LogItemsContainer> GetAsync(FilterOption filterOption = null);
    }
}
