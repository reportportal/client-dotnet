using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Responses.Project;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    public interface IProjectResource
    {
        /// <summary>
        /// Returns information about current accosiated project.
        /// </summary>
        /// <returns>Project info.</returns>
        Task<ProjectResponse> GetAsync();

        /// <summary>
        /// Returns information about project filtered by name.
        /// </summary>
        /// <returns>Project info.</returns>
        Task<ProjectResponse> GetAsync(string projectName);

        /// <summary>
        /// Updates the project preference for user.
        /// </summary>
        Task<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId);

        /// <summary>
        /// Gets all user preferences.
        /// </summary>
        Task<PreferenceResponse> GetAllPreferencesAsync(string projectName, string userName);
    }
}
