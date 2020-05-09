using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceUserFilterResource : ServiceBaseResource, IUserFilterResource
    {
        public ServiceUserFilterResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public async Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request)
        {
            return await PostAsJsonAsync<UserFilterCreatedResponse, CreateUserFilterRequest>($"{ProjectName}/filter", request);
        }

        public async Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption = null)
        {
            var uri = $"{ProjectName}/filter";
            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<UserFilterResponse>>(uri);
        }

        public async Task<UserFilterResponse> GetAsync(long id)
        {
            return await GetAsJsonAsync<UserFilterResponse>($"{ProjectName}/filter/{id}");
        }

        public async Task<MessageResponse> DeleteAsync(long id)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/filter/{id}");
        }
    }
}
