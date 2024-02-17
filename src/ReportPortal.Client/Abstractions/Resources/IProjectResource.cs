using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Responses.Project;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    public interface IProjectResource
    {
        /// <summary>
        /// Returns information about the current associated project.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Project info.</returns>
        Task<ProjectResponse> GetAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns information about a project filtered by name.
        /// </summary>
        /// <param name="projectName">Name of the specified project.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Project info.</returns>
        Task<ProjectResponse> GetAsync(string projectName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the project preference for a user.
        /// </summary>
        /// <param name="projectName">Name of the specified project.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="filterId">Id of the specified filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Message with info.</returns>
        Task<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all user preferences.
        /// </summary>
        /// <param name="projectName">Name of the specified project.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Preference info.</returns>
        Task<PreferenceResponse> GetAllPreferencesAsync(string projectName, string userName, CancellationToken cancellationToken = default);
    }
}
