using System.Threading.Tasks;
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

        IUserFilterResource UserFilter { get; }


        Task<Preference> GetAllPreferences(string userName);
        Task<Service.UpdatePreferencesResponse> UpdatePreferencesAsync(UpdatePreferenceRequest model, string userName);
    }
}