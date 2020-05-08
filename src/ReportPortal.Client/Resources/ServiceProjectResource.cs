using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceProjectResource : ServiceBaseResource, IProjectResource
    {
        public ServiceProjectResource(HttpClient httpClient, string project) : base(httpClient, project)
        {

        }

        public async Task<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId)
        {
            return await PutAsJsonAsync<MessageResponse, object>(
                $"project/{projectName}/preference/{userName}/{filterId}",
                string.Empty);
        }

        public async Task<PreferenceResponse> GetAllPreferences(string projectName, string userName)
        {
            return await GetAsJsonAsync<PreferenceResponse>($"project/{projectName}/preference/{userName}");
        }
    }
}
