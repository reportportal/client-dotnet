using ReportPortal.Client.Abstractions.Responses;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    public interface IProjectResource
    {
        /// <summary>
        /// Updates the project preference for user.
        /// </summary>
        Task<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId);

        /// <summary>
        /// Gets all user preferences.
        /// </summary>
        Task<PreferenceResponse> GetAllPreferences(string projectName, string userName);
    }
}
