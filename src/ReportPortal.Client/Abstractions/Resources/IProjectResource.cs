using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Responses.Project;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    public interface IProjectResource
    {
        /// <summary>
        /// Returns information about current accosiated project.
        /// </summary>
        /// <returns>Project info.</returns>
        ValueTask<ProjectResponse> GetAsync();

        /// <inheritdoc cref="GetAsync()"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<ProjectResponse> GetAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns information about project filtered by name.
        /// </summary>
        /// <param name="projectName">Name of specified project.</param>
        /// <returns>Project info.</returns>
        ValueTask<ProjectResponse> GetAsync(string projectName);

        /// <inheritdoc cref="GetAsync(string)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<ProjectResponse> GetAsync(string projectName, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the project preference for user.
        /// </summary>
        /// <param name="projectName">Name of specified project.</param>
        /// <param name="userName">Name of user.</param>
        /// <param name="filterId">Id of specified filter.</param>
        /// <returns>Message with info.</returns>
        ValueTask<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId);

        /// <inheritdoc cref="UpdatePreferencesAsync(string, string, long)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all user preferences.
        /// </summary>
        /// <param name="projectName">Name of specified project.</param>
        /// <param name="userName">Name of user.</param>
        /// <returns>Preference info.</returns>
        ValueTask<PreferenceResponse> GetAllPreferencesAsync(string projectName, string userName);

        /// <inheritdoc cref="GetAllPreferencesAsync(string, string)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<PreferenceResponse> GetAllPreferencesAsync(string projectName, string userName, CancellationToken cancellationToken);
    }
}
