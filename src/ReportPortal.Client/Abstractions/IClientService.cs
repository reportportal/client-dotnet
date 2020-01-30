using System.Collections.Generic;
using System.Threading.Tasks;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace ReportPortal.Client.Abstractions
{
    public interface IClientService
    {
        ILaunchResource Launch { get; }

        ITestItemResource TestItem { get; }

        ILogItemResource LogItem { get; }

        IUserResource User { get; }

        Task<List<EntryCreated>> AddUserFilterAsync(AddUserFilterRequest model);
        Task<Message> DeleteUserFilterAsync(string filterId);
        Task<Preference> GetAllPreferences(string userName);
        Task<UserFilterContainer> GetUserFiltersAsync(FilterOption filterOption = null);
        Task<Service.UpdatePreferencesResponse> UpdatePreferencesAsync(UpdatePreferenceRequest model, string userName);
    }
}