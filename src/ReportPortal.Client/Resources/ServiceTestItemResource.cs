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

        public async Task<Content<TestItemResponse>> GetAsync(FilterOption filterOption = null)
        {
            return await GetAsync(CancellationToken.None, filterOption).ConfigureAwait(false);
        }

        public async Task<Content<TestItemResponse>> GetAsync(CancellationToken cancellationToken, FilterOption filterOption = null)
        {
            var uri = $"{ProjectName}/item";
            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<TestItemResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestItemResponse> GetAsync(long id)
        {
            return await GetAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<TestItemResponse> GetAsync(long id, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<TestItemResponse>($"{ProjectName}/item/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestItemResponse> GetAsync(string uuid)
        {
            return await GetAsync(uuid, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<TestItemResponse> GetAsync(string uuid, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<TestItemResponse>($"{ProjectName}/item/uuid/{uuid}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request)
        {
            return await StartAsync(request, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"{ProjectName}/item", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model)
        {
            return await StartAsync(uuid, model, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"{ProjectName}/item/{uuid}", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request)
        {
            return await FinishAsync(uuid, request, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, FinishTestItemRequest>(
                $"{ProjectName}/item/{uuid}", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request)
        {
            return await UpdateAsync(id, request, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateTestItemRequest>(
                $"{ProjectName}/item/{id}/update", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> DeleteAsync(long id)
        {
            return await DeleteAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/item/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request)
        {
            return await AssignIssuesAsync(request, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<IEnumerable<Issue>, AssignTestItemIssuesRequest>(
                $"{ProjectName}/item", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth)
        {
            return await GetHistoryAsync(id, depth, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth, CancellationToken cancellationToken)
        {
            var uri = $"{ProjectName}/item/history?filter.eq.id={id}&type=line&historyDepth={depth}";

            return await GetAsJsonAsync<Content<TestItemHistoryContainer>>(uri, cancellationToken).ConfigureAwait(false);
        }
    }
}
