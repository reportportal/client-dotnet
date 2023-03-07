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

        public Task<Content<UserFilterResponse>> GetAsync()
        {
            return GetAsync(filterOption: null, CancellationToken.None);
        }

        public Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption)
        {
            return GetAsync(filterOption, CancellationToken.None);
        }

        public async Task<Content<UserFilterResponse>> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync(filterOption: null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            var uri = $"v1/{ProjectName}/filter";
            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<UserFilterResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }

        public Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request)
        {
            return CreateAsync(request, CancellationToken.None);
        }

        public async Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<UserFilterCreatedResponse, CreateUserFilterRequest>(
                $"v1/{ProjectName}/filter", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<MessageResponse> UpdateAsync(long id, UpdateUserFilterRequest request)
        {
            return UpdateAsync(id, request, CancellationToken.None);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateUserFilterRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateUserFilterRequest>(
                $"v1/{ProjectName}/filter/{id}", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<UserFilterResponse> GetAsync(long id)
        {
            return GetAsync(id, CancellationToken.None);
        }

        public async Task<UserFilterResponse> GetAsync(long id, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<UserFilterResponse>($"v1/{ProjectName}/filter/{id}", cancellationToken).ConfigureAwait(false);
        }

        public Task<MessageResponse> DeleteAsync(long id)
        {
            return DeleteAsync(id, CancellationToken.None);
        }

        public async Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"v1/{ProjectName}/filter/{id}", cancellationToken).ConfigureAwait(false);
        }
    }
}
