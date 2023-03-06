using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceTestItemResource : ServiceBaseResource, ITestItemResource
    {
        public ServiceTestItemResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public Task<Content<TestItemResponse>> GetAsync()
        {
            return GetAsync(filterOption: null, CancellationToken.None);
        }

        public Task<Content<TestItemResponse>> GetAsync(FilterOption filterOption)
        {
            return GetAsync(filterOption, CancellationToken.None);
        }

        public async Task<Content<TestItemResponse>> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync(filterOption: null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Content<TestItemResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            var uri = $"v1/{ProjectName}/item";
            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<TestItemResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }

        public Task<TestItemResponse> GetAsync(long id)
        {
            return GetAsync(id, CancellationToken.None);
        }

        public async Task<TestItemResponse> GetAsync(long id, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<TestItemResponse>($"v1/{ProjectName}/item/{id}", cancellationToken).ConfigureAwait(false);
        }

        public Task<TestItemResponse> GetAsync(string uuid)
        {
            return GetAsync(uuid, CancellationToken.None);
        }

        public async Task<TestItemResponse> GetAsync(string uuid, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<TestItemResponse>($"v1/{ProjectName}/item/uuid/{uuid}", cancellationToken).ConfigureAwait(false);
        }

        public Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request)
        {
            return StartAsync(request, CancellationToken.None);
        }

        public async Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"v1/{ProjectName}/item", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model)
        {
            return StartAsync(uuid, model, CancellationToken.None);
        }

        public async Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"v1/{ProjectName}/item/{uuid}", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request)
        {
            return FinishAsync(uuid, request, CancellationToken.None);
        }

        public async Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, FinishTestItemRequest>(
                $"v1/{ProjectName}/item/{uuid}", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request)
        {
            return UpdateAsync(id, request, CancellationToken.None);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateTestItemRequest>(
                $"v1/{ProjectName}/item/{id}/update", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<MessageResponse> DeleteAsync(long id)
        {
            return DeleteAsync(id, CancellationToken.None);
        }

        public async Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"v1/{ProjectName}/item/{id}", cancellationToken).ConfigureAwait(false);
        }

        public Task<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request)
        {
            return AssignIssuesAsync(request, CancellationToken.None);
        }

        public async Task<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<IEnumerable<Issue>, AssignTestItemIssuesRequest>(
                $"v1/{ProjectName}/item", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth)
        {
            return GetHistoryAsync(id, depth, CancellationToken.None);
        }

        public async Task<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth, CancellationToken cancellationToken)
        {
            var uri = $"v1/{ProjectName}/item/history?filter.eq.id={id}&type=line&historyDepth={depth}";

            return await GetAsJsonAsync<Content<TestItemHistoryContainer>>(uri, cancellationToken).ConfigureAwait(false);
        }
    }
}
