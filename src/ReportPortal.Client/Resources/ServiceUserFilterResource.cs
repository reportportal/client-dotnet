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

        public async Task<Content<UserFilterResponse>> GetAsync()
        {
            return await GetAsync(filterOption: null, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption)
        {
            return await GetAsync(filterOption, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<Content<UserFilterResponse>> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync(filterOption: null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            var uri = $"{ProjectName}/filter";
            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<UserFilterResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }

        public async Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request)
        {
            return await CreateAsync(request, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<UserFilterCreatedResponse, CreateUserFilterRequest>(
                $"{ProjectName}/filter", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateUserFilterRequest request)
        {
            return await UpdateAsync(id, request, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateUserFilterRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateUserFilterRequest>(
                $"{ProjectName}/filter/{id}", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<UserFilterResponse> GetAsync(long id)
        {
            return await GetAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<UserFilterResponse> GetAsync(long id, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<UserFilterResponse>($"{ProjectName}/filter/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> DeleteAsync(long id)
        {
            return await DeleteAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/filter/{id}", cancellationToken).ConfigureAwait(false);
        }
    }
}
