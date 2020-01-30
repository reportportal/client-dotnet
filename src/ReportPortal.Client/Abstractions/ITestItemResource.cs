using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions
{
    public interface ITestItemResource
    {
        Task<List<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest model);
        Task<Message> DeleteAsync(long id);
        Task<Message> FinishAsync(string id, FinishTestItemRequest model);
        Task<TestItem> GetAsync(long id);
        Task<TestItem> GetAsync(string uuid);
        Task<List<TestItemHistory>> GetHistoryAsync(IEnumerable<long> testItemIds, int depth, bool full);
        Task<TestItemsContainer> GetAsync(FilterOption filterOption = null);
        Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest model);
        Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model);
        Task<Message> UpdateAsync(long id, UpdateTestItemRequest model);
    }
}
