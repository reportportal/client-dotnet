using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions
{
    public interface ITestItemResource
    {
        Task<List<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest model);
        Task<MessageResponse> DeleteAsync(long id);
        Task<MessageResponse> FinishAsync(string id, FinishTestItemRequest model);
        Task<TestItemResponse> GetAsync(long id);
        Task<TestItemResponse> GetAsync(string uuid);
        Task<List<TestItemHistoryModel>> GetHistoryAsync(IEnumerable<long> testItemIds, int depth, bool full);
        Task<Content<TestItemResponse>> GetAsync(FilterOption filterOption = null);
        Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest model);
        Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model);
        Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest model);
    }
}
