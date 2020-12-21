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

        public Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request)
        {
            return PostAsJsonAsync<UserFilterCreatedResponse, CreateUserFilterRequest>($"{ProjectName}/filter", request);
        }

        public  Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption = null)
        {
            var uri = $"{ProjectName}/filter";
            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return GetAsJsonAsync<Content<UserFilterResponse>>(uri);
        }

        public Task<UserFilterResponse> GetAsync(long id)
        {
            return GetAsJsonAsync<UserFilterResponse>($"{ProjectName}/filter/{id}");
        }

        public Task<MessageResponse> DeleteAsync(long id)
        {
            return DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/filter/{id}");
        }
    }
}
