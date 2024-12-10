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

        public async Task<TestItemResponse> GetAsync(long id, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<TestItemResponse>($"v1/{ProjectName}/item/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestItemResponse> GetAsync(string uuid, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<TestItemResponse>($"v1/{ProjectName}/item/uuid/{uuid}", "application/x.reportportal.test.v2+json", cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"v1/{ProjectName}/item", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"v1/{ProjectName}/item/{uuid}", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, FinishTestItemRequest>(
                $"v1/{ProjectName}/item/{uuid}", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateTestItemRequest>(
                $"v1/{ProjectName}/item/{id}/update", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"v1/{ProjectName}/item/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<IList<Issue>, AssignTestItemIssuesRequest>(
                $"v1/{ProjectName}/item", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth, CancellationToken cancellationToken)
        {
            var uri = $"v1/{ProjectName}/item/history?filter.eq.id={id}&type=line&historyDepth={depth}";

            return await GetAsJsonAsync<Content<TestItemHistoryContainer>>(uri, cancellationToken).ConfigureAwait(false);
        }
    }
}
