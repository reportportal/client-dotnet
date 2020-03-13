using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Extentions;
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
            var uri = $"project/{projectName}/preference/{userName}/{filterId}";

            var response = await HttpClient.PutAsync(uri, new StringContent(string.Empty)).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<PreferenceResponse> GetAllPreferences(string projectName, string userName)
        {
            var uri = $"project/{projectName}/preference/{userName}";

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<PreferenceResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
