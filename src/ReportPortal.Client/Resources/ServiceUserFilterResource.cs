using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceUserFilterResource : ServiceBaseResource, IUserFilterResource
    {
        public ServiceUserFilterResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public async Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request, CancellationToken cancellationToken = default)
        {
            return await PostAsJsonAsync<UserFilterCreatedResponse, CreateUserFilterRequest>(
                $"{ProjectName}/filter", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption = null, CancellationToken cancellationToken = default)
        {
            var uri = $"{ProjectName}/filter";
            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<UserFilterResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateUserFilterRequest request, CancellationToken cancellationToken = default)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateUserFilterRequest>(
                $"{ProjectName}/filter/{id}", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<UserFilterResponse> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            return await GetAsJsonAsync<UserFilterResponse>($"{ProjectName}/filter/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/filter/{id}", cancellationToken).ConfigureAwait(false);
        }
    }
}
