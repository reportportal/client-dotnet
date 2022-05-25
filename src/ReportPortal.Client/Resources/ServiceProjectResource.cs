using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Responses.Project;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceProjectResource : ServiceBaseResource, IProjectResource
    {
        public ServiceProjectResource(HttpClient httpClient, string project) : base(httpClient, project)
        {

        }

        public async Task<ProjectResponse> GetAsync()
        {
            return await GetAsJsonAsync<ProjectResponse>($"project/{this.ProjectName}").ConfigureAwait(false);
        }

        public async Task<ProjectResponse> GetAsync(string projectName)
        {
            return await GetAsJsonAsync<ProjectResponse>($"project/{projectName}").ConfigureAwait(false);
        }

        public async Task<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId)
        {
            return await PutAsJsonAsync<MessageResponse, object>(
                $"project/{projectName}/preference/{userName}/{filterId}",
                string.Empty).ConfigureAwait(false);
        }

        public async Task<PreferenceResponse> GetAllPreferencesAsync(string projectName, string userName)
        {
            return await GetAsJsonAsync<PreferenceResponse>($"project/{projectName}/preference/{userName}").ConfigureAwait(false);
        }
    }
}
