using System.Collections.Generic;
using System.Threading.Tasks;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Converters;
using System;
using System.Net.Http;
using System.Text;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions;

namespace ReportPortal.Client
{
    public class ServiceTestItemResource : BaseResource, ITestItemResource
    {
        public ServiceTestItemResource(HttpClient httpClient, Uri baseUri, string project, string token) : base(httpClient, baseUri, project, token)
        {

        }
        /// <summary>
        /// Returns a list of test items for specified launch and parent test item (optional).
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving test items.</param>
        /// <returns>A list of test items.</returns>
        public virtual async Task<TestItemsContainer> GetAsync(FilterOption filterOption = null)
        {
            var uri = BaseUri.Append($"{Project}/item");
            if (filterOption != null)
            {
                uri = uri.Append($"?{filterOption}");
            }
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemsContainer>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Returns specified test item by ID.
        /// </summary>
        /// <param name="id">ID of the test item to retrieve.</param>
        /// <returns>A representation of test item.</returns>
        public virtual async Task<TestItemModel> GetAsync(long id)
        {
            var uri = BaseUri.Append($"{Project}/item/{id}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Returns specified test item by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the test item to retrieve.</param>
        /// <returns>A representation of test item.</returns>
        public virtual async Task<Abstractions.Responses.TestItemModel> GetAsync(string uuid)
        {
            var uri = BaseUri.Append($"{Project}/item/uuid/{uuid}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="model">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        public virtual async Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest model)
        {
            var uri = BaseUri.Append($"{Project}/item");
            var body = ModelSerializer.Serialize<StartTestItemRequest>(model);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemCreatedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="uuid">UUID of parent item.</param>
        /// <param name="model">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        public virtual async Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model)
        {
            var uri = BaseUri.Append($"{Project}/item/{uuid}");
            var body = ModelSerializer.Serialize<StartTestItemRequest>(model);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemCreatedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Finishes specified test item.
        /// </summary>
        /// <param name="id">ID of specified test item.</param>
        /// <param name="model">Information about representation of test item to finish.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<Message> FinishAsync(string id, FinishTestItemRequest model)
        {
            var uri = BaseUri.Append($"{Project}/item/{id}");
            var body = ModelSerializer.Serialize<FinishTestItemRequest>(model);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Update specified test item.
        /// </summary>
        /// <param name="id">ID of test item to update.</param>
        /// <param name="model">Information about test item.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<Message> UpdateAsync(long id, UpdateTestItemRequest model)
        {
            var uri = BaseUri.Append($"{Project}/item/{id}/update");
            var body = ModelSerializer.Serialize<UpdateTestItemRequest>(model);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Deletes specified test item.
        /// </summary>
        /// <param name="id">ID of the test item to delete.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<Message> DeleteAsync(long id)
        {
            var uri = BaseUri.Append($"{Project}/item/{id}");
            var response = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Assign issues to specified test items.
        /// </summary>
        /// <param name="model">Information about test items and their issues.</param>
        /// <returns>A list of assigned issues.</returns>
        public virtual async Task<List<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest model)
        {
            var uri = BaseUri.Append($"{Project}/item");
            var body = ModelSerializer.Serialize<AssignTestItemIssuesRequest>(model);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<List<Issue>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Get the history of test items executions.
        /// </summary>
        /// <param name="testItemIds">IDs of test items.</param>
        /// <param name="depth">How many executions to return.</param>
        /// <param name="full"></param>
        /// <returns>The list of execution history.</returns>
        public virtual async Task<List<TestItemHistoryModel>> GetHistoryAsync(IEnumerable<long> testItemIds, int depth, bool full)
        {
            var uri = BaseUri.Append($"{Project}/item/history?ids={string.Join(",", testItemIds)}&history_depth={depth}&is_full={full}");

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<List<TestItemHistoryModel>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
