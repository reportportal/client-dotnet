using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Responses.Project;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceProjectResource : ServiceBaseResource, IProjectResource
    {
        public ServiceProjectResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public Task<ProjectResponse> GetAsync()
        {
            return GetAsync(CancellationToken.None);
        }

        public async Task<ProjectResponse> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<ProjectResponse>($"project/{ProjectName}", cancellationToken).ConfigureAwait(false);
        }

        public Task<ProjectResponse> GetAsync(string projectName)
        {
            return GetAsync(projectName, CancellationToken.None);
        }

        public async Task<ProjectResponse> GetAsync(string projectName, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<ProjectResponse>($"project/{projectName}", cancellationToken).ConfigureAwait(false);
        }

        public Task<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId)
        {
            return UpdatePreferencesAsync(projectName, userName, filterId, CancellationToken.None);
        }

        public async Task<MessageResponse> UpdatePreferencesAsync(
            string projectName, string userName, long filterId, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, object>(
                $"project/{projectName}/preference/{userName}/{filterId}",
                string.Empty,
                cancellationToken).ConfigureAwait(false);
        }

        public Task<PreferenceResponse> GetAllPreferencesAsync(string projectName, string userName)
        {
            return GetAllPreferencesAsync(projectName, userName, CancellationToken.None);
        }

        public async Task<PreferenceResponse> GetAllPreferencesAsync(
            string projectName, string userName, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<PreferenceResponse>(
                $"project/{projectName}/preference/{userName}", cancellationToken).ConfigureAwait(false);
        }
    }
}
