using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Collections.Generic;
using System.Net.Http;
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
            var uri = $"{ProjectName}/item";
            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<TestItemResponse>>(uri).ConfigureAwait(false);
        }

        public async Task<TestItemResponse> GetAsync(long id)
        {
            return await GetAsJsonAsync<TestItemResponse>($"{ProjectName}/item/{id}").ConfigureAwait(false);
        }

        public async Task<TestItemResponse> GetAsync(string uuid)
        {
            return await GetAsJsonAsync<TestItemResponse>($"{ProjectName}/item/uuid/{uuid}").ConfigureAwait(false);
        }

        public async Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"{ProjectName}/item",
                request).ConfigureAwait(false);
        }

        public async Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest request)
        {
            return await PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"{ProjectName}/item/{uuid}",
                request).ConfigureAwait(false);
        }

        public async Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request)
        {
            return await PutAsJsonAsync<MessageResponse, FinishTestItemRequest>(
                $"{ProjectName}/item/{uuid}",
                request).ConfigureAwait(false);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateTestItemRequest>(
                $"{ProjectName}/item/{id}/update",
                request).ConfigureAwait(false);
        }

        public async Task<MessageResponse> DeleteAsync(long id)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/item/{id}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request)
        {
            return await PutAsJsonAsync<IEnumerable<Issue>, AssignTestItemIssuesRequest>(
                $"{ProjectName}/item",
                request).ConfigureAwait(false);
        }

        public async Task<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth)
        {
            var uri = $"{ProjectName}/item/history?filter.eq.id={id}&type=line&historyDepth={depth}";

            return await GetAsJsonAsync<Content<TestItemHistoryContainer>>(uri).ConfigureAwait(false);
        }
    }
}
