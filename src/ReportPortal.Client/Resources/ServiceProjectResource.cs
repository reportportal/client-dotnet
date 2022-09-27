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

        public async ValueTask<ProjectResponse> GetAsync()
        {
            return await GetAsync(CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<ProjectResponse> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<ProjectResponse>($"project/{ProjectName}", cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<ProjectResponse> GetAsync(string projectName)
        {
            return await GetAsync(projectName, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<ProjectResponse> GetAsync(string projectName, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<ProjectResponse>($"project/{projectName}", cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId)
        {
            return await UpdatePreferencesAsync(projectName, userName, filterId, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<MessageResponse> UpdatePreferencesAsync(
            string projectName, string userName, long filterId, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, object>(
                $"project/{projectName}/preference/{userName}/{filterId}",
                string.Empty,
                cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<PreferenceResponse> GetAllPreferencesAsync(
            string projectName, string userName, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<PreferenceResponse>(
                $"project/{projectName}/preference/{userName}", cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<PreferenceResponse> GetAllPreferencesAsync(string projectName, string userName)
        {
            return await GetAllPreferencesAsync(projectName, userName, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
