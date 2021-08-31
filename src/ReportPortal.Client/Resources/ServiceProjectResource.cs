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

        public Task<ProjectResponse> GetAsync()
        {
            return GetAsJsonAsync<ProjectResponse>($"project/{this.ProjectName}");
        }

        public Task<ProjectResponse> GetAsync(string projectName)
        {
            return GetAsJsonAsync<ProjectResponse>($"project/{projectName}");
        }

        public Task<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId)
        {
            return PutAsJsonAsync<MessageResponse, object>(
                $"project/{projectName}/preference/{userName}/{filterId}",
                string.Empty);
        }

        public Task<PreferenceResponse> GetAllPreferencesAsync(string projectName, string userName)
        {
            return GetAsJsonAsync<PreferenceResponse>($"project/{projectName}/preference/{userName}");
        }
    }
}
