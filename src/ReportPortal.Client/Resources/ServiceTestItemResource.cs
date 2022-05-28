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

        public async Task<Content<TestItemResponse>> GetAsync(FilterOption filterOption = null, CancellationToken cancellationToken = default)
        {
            var uri = $"{ProjectName}/item";
            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<TestItemResponse>>(uri).ConfigureAwait(false);
        }

        public async Task<TestItemResponse> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            return await GetAsJsonAsync<TestItemResponse>($"{ProjectName}/item/{id}").ConfigureAwait(false);
        }

        public async Task<TestItemResponse> GetAsync(string uuid, CancellationToken cancellationToken = default)
        {
            return await GetAsJsonAsync<TestItemResponse>($"{ProjectName}/item/uuid/{uuid}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request, CancellationToken cancellationToken = default)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"{ProjectName}/item", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestItemCreatedResponse> StartAsync(
            string uuid, StartTestItemRequest request, CancellationToken cancellationToken = default)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"{ProjectName}/item/{uuid}", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request, CancellationToken cancellationToken = default)
        {
            return await PutAsJsonAsync<MessageResponse, FinishTestItemRequest>(
                $"{ProjectName}/item/{uuid}", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request, CancellationToken cancellationToken = default)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateTestItemRequest>(
                $"{ProjectName}/item/{id}/update", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/item/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request, CancellationToken cancellationToken = default)
        {
            return await PutAsJsonAsync<IEnumerable<Issue>, AssignTestItemIssuesRequest>(
                $"{ProjectName}/item", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth, CancellationToken cancellationToken = default)
        {
            var uri = $"{ProjectName}/item/history?filter.eq.id={id}&type=line&historyDepth={depth}";

            return await GetAsJsonAsync<Content<TestItemHistoryContainer>>(uri, cancellationToken).ConfigureAwait(false);
        }
    }
}
