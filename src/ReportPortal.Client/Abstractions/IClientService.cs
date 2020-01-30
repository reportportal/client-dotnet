using System.Collections.Generic;
using System.Threading.Tasks;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Client.Responses;

namespace ReportPortal.Client.Abstractions
{
    public interface IClientService
    {
        ILaunchResource Launch { get; }

        ITestItemResource TestItem { get; }

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