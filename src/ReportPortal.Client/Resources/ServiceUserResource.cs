using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceUserResource : ServiceBaseResource, IUserResource
    {
        public ServiceUserResource(HttpClient httpClient, string project) : base(httpClient, project)
        {

        }

        public async Task<UserResponse> GetAsync()
        {
            return await GetAsJsonAsync<UserResponse>("user");
        }
    }
}
