using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Extentions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceUserFilterResource : ServiceBaseResource, IUserFilterResource
    {
        public ServiceUserFilterResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public async Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest model)
        {
            var uri = $"{ProjectName}/filter";

            var body = ModelSerializer.Serialize<CreateUserFilterRequest>(model);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<UserFilterCreatedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption = null)
        {
            var uri = $"{ProjectName}/filter";
            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Content<UserFilterResponse>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<UserFilterResponse> GetAsync(long id)
        {
            var uri = $"{ProjectName}/filter/{id}";

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<UserFilterResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<MessageResponse> DeleteAsync(long id)
        {
            var uri = $"{ProjectName}/filter/{id}";

            var response = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
