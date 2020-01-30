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
        Task<List<Issue>> AssignTestItemIssuesAsync(AssignTestItemIssuesRequest model);
        Task<Message> DeleteTestItemAsync(long id);
        Task<Message> FinishTestItemAsync(string id, FinishTestItemRequest model);
        Task<TestItem> GetTestItemAsync(long id);
        Task<TestItem> GetTestItemAsync(string uuid);
        Task<List<TestItemHistory>> GetTestItemHistoryAsync(IEnumerable<long> testItemIds, int depth, bool full);
        Task<TestItemsContainer> GetTestItemsAsync(FilterOption filterOption = null);
        Task<TestItemCreatedResponse> StartTestItemAsync(StartTestItemRequest model);
        Task<TestItemCreatedResponse> StartTestItemAsync(string uuid, StartTestItemRequest model);
        Task<Message> UpdateTestItemAsync(long id, UpdateTestItemRequest model);
    }
}
