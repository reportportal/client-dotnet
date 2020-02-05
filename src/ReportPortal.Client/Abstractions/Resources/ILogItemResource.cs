using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    public interface ILogItemResource
    {
        Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest model);
        Task<MessageResponse> DeleteAsync(long id);
        Task<byte[]> GetBinaryDataAsync(string id);
        Task<LogItemResponse> GetAsync(long id);
        Task<LogItemResponse> GetAsync(string uuid);
        Task<Content<LogItemResponse>> GetAsync(FilterOption filterOption = null);
    }
}
