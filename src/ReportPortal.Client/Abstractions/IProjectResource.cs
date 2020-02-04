using ReportPortal.Client.Abstractions.Responses;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions
{
    public interface IProjectResource
    {
        Task<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId);

        Task<PreferenceResponse> GetAllPreferences(string projectName, string userName);
    }
}
