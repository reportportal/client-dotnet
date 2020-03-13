using System.Threading.Tasks;
using ReportPortal.Client.Converters;
using System.Net.Http;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Resources;

namespace ReportPortal.Client.Resources
{
    class ServiceUserResource : ServiceBaseResource, IUserResource
    {
        public ServiceUserResource(HttpClient httpClient, string project) : base(httpClient, project)
        {

        }

        public async Task<UserResponse> GetAsync()
        {
            var uri = "user";
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<UserResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
