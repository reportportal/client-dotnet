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

        public Task<Content<TestItemResponse>> GetAsync(FilterOption filterOption = null)
        {
            var uri = $"{ProjectName}/item";
            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return GetAsJsonAsync<Content<TestItemResponse>>(uri);
        }

        public Task<TestItemResponse> GetAsync(long id)
        {
            return GetAsJsonAsync<TestItemResponse>($"{ProjectName}/item/{id}");
        }

        public Task<TestItemResponse> GetAsync(string uuid)
        {
            return GetAsJsonAsync<TestItemResponse>($"{ProjectName}/item/uuid/{uuid}");
        }

        public Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request)
        {
            return PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"{ProjectName}/item",
                request);
        }

        public Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest request)
        {
            return PostAsJsonAsync<TestItemCreatedResponse, StartTestItemRequest>(
                $"{ProjectName}/item/{uuid}",
                request);
        }

        public Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request)
        {
            return PutAsJsonAsync<MessageResponse, FinishTestItemRequest>(
                $"{ProjectName}/item/{uuid}",
                request);
        }

        public Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request)
        {
            return PutAsJsonAsync<MessageResponse, UpdateTestItemRequest>(
                $"{ProjectName}/item/{id}/update",
                request);
        }

        public Task<MessageResponse> DeleteAsync(long id)
        {
            return DeleteAsJsonAsync<MessageResponse>($"{ProjectName}/item/{id}");
        }

        public Task<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request)
        {
            return PutAsJsonAsync<IEnumerable<Issue>, AssignTestItemIssuesRequest>(
                $"{ProjectName}/item",
                request);
        }

        public Task<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth)
        {
            var uri = $"{ProjectName}/item/history?filter.eq.id={id}&type=line&historyDepth={depth}";

            return GetAsJsonAsync<Content<TestItemHistoryContainer>>(uri);
        }
    }
}
