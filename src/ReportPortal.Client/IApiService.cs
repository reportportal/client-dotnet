using System.Collections.Generic;
using System.Threading.Tasks;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Client.Responses;

namespace ReportPortal.Client
{
    public interface IApiService
    {
        #region Launch
        Task<Message> AnalyzeLaunchAsync(AnalyzeLaunchRequest model);
        Task<LaunchFinishedResponse> FinishLaunchAsync(string uuid, FinishLaunchRequest model);
        Task<Message> DeleteLaunchAsync(long id);
        Task<Launch> GetLaunchAsync(long id);
        Task<Launch> GetLaunchAsync(string uuid);
        Task<LaunchesContainer> GetLaunchesAsync(FilterOption filterOption = null, bool debug = false);
        Task<Launch> MergeLaunchesAsync(MergeLaunchesRequest model);
        Task<LaunchCreatedResponse> StartLaunchAsync(StartLaunchRequest request);
        Task<LaunchFinishedResponse> StopLaunchAsync(long id, FinishLaunchRequest model);
        Task<Message> UpdateLaunchAsync(long id, UpdateLaunchRequest model);
        #endregion

        #region Test
        Task<List<Issue>> AssignTestItemIssuesAsync(AssignTestItemIssuesRequest model);
        Task<Message> DeleteTestItemAsync(long id);
        Task<Message> FinishTestItemAsync(string id, FinishTestItemRequest model);
        Task<TestItem> GetTestItemAsync(long id);
        Task<TestItem> GetTestItemAsync(string uuid);
        Task<List<TestItemHistory>> GetTestItemHistoryAsync(string testItemId, int depth, bool full);
        Task<TestItemsContainer> GetTestItemsAsync(FilterOption filterOption = null);
        Task<TestItemCreatedResponse> StartTestItemAsync(StartTestItemRequest model);
        Task<TestItemCreatedResponse> StartTestItemAsync(string uuid, StartTestItemRequest model);
        Task<Message> UpdateTestItemAsync(long id, UpdateTestItemRequest model);
        #endregion

        #region Log
        Task<LogItemCreatedResponse> AddLogItemAsync(AddLogItemRequest model);
        Task<Message> DeleteLogItemAsync(long id);
        Task<byte[]> GetBinaryDataAsync(string id);
        Task<LogItem> GetLogItemAsync(long id);
        Task<LogItem> GetLogItemAsync(string uuid);
        Task<LogItemsContainer> GetLogItemsAsync(FilterOption filterOption = null);
        #endregion

        Task<List<EntryCreated>> AddUserFilterAsync(AddUserFilterRequest model);
        Task<Message> DeleteUserFilterAsync(string filterId);
        Task<Preference> GetAllPreferences(string userName);
        Task<User> GetUserAsync();
        Task<UserFilterContainer> GetUserFiltersAsync(FilterOption filterOption = null);
        Task<Service.UpdatePreferencesResponse> UpdatePreferencesAsync(UpdatePreferenceRequest model, string userName);
    }
}