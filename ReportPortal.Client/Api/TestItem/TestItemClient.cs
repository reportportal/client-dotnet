using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Api.TestItem.Model;
using ReportPortal.Client.Api.TestItem.Request;
using ReportPortal.Client.Common.Model;
using ReportPortal.Client.Common.Model.Filtering;
using ReportPortal.Client.Common.Model.Paging;
using ReportPortal.Client.Converter;
using ReportPortal.Client.Extention;

namespace ReportPortal.Client.Api.TestItem
{
    public class TestItemApiClient: BaseApiClient, ITestItemApiClient
    {
        public TestItemApiClient(HttpClient httpCLient, Uri baseUri, string project) : base(httpCLient, baseUri, project)
        {
        }

        public virtual async Task<PagingContent<TestItemModel>> GetTestItemsAsync(FilterOption filterOption = null)
        {
            var uri = BaseUri.Append($"{Project}/item");
            if (filterOption != null)
            {
                uri = uri.Append($"?{filterOption}");
            }
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<PagingContent<TestItemModel>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<TestItemModel> GetTestItemAsync(string id)
        {
            var uri = BaseUri.Append($"{Project}/item/{id}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<List<string>> GetUniqueTagsAsync(string launchId, string tagContains)
        {
            var uri = BaseUri.Append($"{Project}/item/tags?launch={launchId}&filter.cnt.tags={tagContains}");

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<List<string>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<TestItemModel> StartTestItemAsync(StartTestItemRequest model)
        {
            var uri = BaseUri.Append($"{Project}/item");
            var body = ModelSerializer.Serialize<StartTestItemRequest>(model);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<TestItemModel> StartTestItemAsync(string id, StartTestItemRequest model)
        {
            var uri = BaseUri.Append($"{Project}/item/{id}");
            var body = ModelSerializer.Serialize<StartTestItemRequest>(model);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Message> FinishTestItemAsync(string id, FinishTestItemRequest model)
        {
            var uri = BaseUri.Append($"{Project}/item/{id}");
            var body = ModelSerializer.Serialize<FinishTestItemRequest>(model);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Message> UpdateTestItemAsync(string id, UpdateTestItemRequest model)
        {
            var uri = BaseUri.Append($"{Project}/item/{id}/update");
            var body = ModelSerializer.Serialize<UpdateTestItemRequest>(model);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Message> DeleteTestItemAsync(string id)
        {
            var uri = BaseUri.Append($"{Project}/item/{id}");
            var response = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<List<Issue>> AssignTestItemIssuesAsync(AssignTestItemIssuesRequest model)
        {
            var uri = BaseUri.Append($"{Project}/item");
            var body = ModelSerializer.Serialize<AssignTestItemIssuesRequest>(model);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<List<Issue>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<List<TestItemHistoryModel>> GetTestItemHistoryAsync(string testItemId, int depth, bool full)
        {
            var uri = BaseUri.Append($"{Project}/item/history?ids={testItemId}&history_depth={depth}&is_full={full}");

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<List<TestItemHistoryModel>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
