using System.Collections.Generic;
using System.Threading.Tasks;
using ReportPortal.Client.Converters;
using System.Net.Http;
using System.Text;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Resources;

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
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Content<TestItemResponse>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<TestItemResponse> GetAsync(long id)
        {
            var uri = $"{ProjectName}/item/{id}";
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<TestItemResponse> GetAsync(string uuid)
        {
            var uri = $"{ProjectName}/item/uuid/{uuid}";
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request)
        {
            var uri = $"{ProjectName}/item";
            var body = ModelSerializer.Serialize<StartTestItemRequest>(request);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemCreatedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest request)
        {
            var uri = $"{ProjectName}/item/{uuid}";
            var body = ModelSerializer.Serialize<StartTestItemRequest>(request);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemCreatedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<MessageResponse> FinishAsync(string id, FinishTestItemRequest request)
        {
            var uri = $"{ProjectName}/item/{id}";
            var body = ModelSerializer.Serialize<FinishTestItemRequest>(request);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request)
        {
            var uri = $"{ProjectName}/item/{id}/update";
            var body = ModelSerializer.Serialize<UpdateTestItemRequest>(request);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<MessageResponse> DeleteAsync(long id)
        {
            var uri = $"{ProjectName}/item/{id}";
            var response = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request)
        {
            var uri = $"{ProjectName}/item";
            var body = ModelSerializer.Serialize<AssignTestItemIssuesRequest>(request);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<IEnumerable<Issue>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<IEnumerable<TestItemHistoryResponse>> GetHistoryAsync(IEnumerable<long> testItemIds, int depth, bool full)
        {
            var uri = $"{ProjectName}/item/history?ids={string.Join(",", testItemIds)}&history_depth={depth}&is_full={full}";

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<IEnumerable<TestItemHistoryResponse>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
