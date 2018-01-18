using System.Collections.Generic;
using System.Threading.Tasks;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Client.Converters;
using System;
using System.Net.Http;
using System.Text;
using ReportPortal.Client.Extentions;

namespace ReportPortal.Client
{
    public partial class Service
    {
        /// <summary>
        /// Returns a list of test items for specified launch and parent test item (optional).
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving test items.</param>
        /// <returns>A list of test items.</returns>
        public async Task<TestItemsContainer> GetTestItemsAsync(FilterOption filterOption = null)
        {
            UriBuilder uriBuilder = new UriBuilder($"{BaseUri}/{Project}/item");
            if (filterOption != null)
            {
                uriBuilder.Query += filterOption;
            }
            var response = await _httpClient.GetAsync(uriBuilder.Uri);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItemsContainer>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Returns specified test item by ID.
        /// </summary>
        /// <param name="id">ID of the test item to retrieve.</param>
        /// <returns>A representation of test item.</returns>
        public async Task<TestItem> GetTestItemAsync(string id)
        {
            var uri = $"{Project}/item/{id}";
            var response = await _httpClient.GetAsync(uri);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItem>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Returns the list of tests tags for specified launch.
        /// </summary>
        /// <param name="launchId">ID of launch.</param>
        /// <param name="tagContains">Tags should contain specified text.</param>
        /// <returns></returns>
        public async Task<List<string>> GetUniqueTagsAsync(string launchId, string tagContains)
        {
            var uri = $"{Project}/item/tags?launch={launchId}&filter.cnt.tags={tagContains}";

            var response = await _httpClient.GetAsync(uri);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<List<string>>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="model">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        public async Task<TestItem> StartTestItemAsync(StartTestItemRequest model)
        {
            var uri = $"{Project}/item";
            var body = ModelSerializer.Serialize<StartTestItemRequest>(model);
            var response = await _httpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json"));
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItem>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="id">ID of parent item.</param>
        /// <param name="model">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        public async Task<TestItem> StartTestItemAsync(string id, StartTestItemRequest model)
        {
            var uri = $"{Project}/item/{id}";
            var body = ModelSerializer.Serialize<StartTestItemRequest>(model);
            var response = await _httpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json"));
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<TestItem>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Finishes specified test item.
        /// </summary>
        /// <param name="id">ID of specified test item.</param>
        /// <param name="model">Information about representation of test item to finish.</param>
        /// <returns>A message from service.</returns>
        public async Task<Message> FinishTestItemAsync(string id, FinishTestItemRequest model)
        {
            var uri = $"{Project}/item/{id}";
            var body = ModelSerializer.Serialize<FinishTestItemRequest>(model);
            var response = await _httpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json"));
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Update specified test item.
        /// </summary>
        /// <param name="id">ID of test item to update.</param>
        /// <param name="model">Information about test item.</param>
        /// <returns>A message from service.</returns>
        public async Task<Message> UpdateTestItemAsync(string id, UpdateTestItemRequest model)
        {
            var uri = $"{Project}/item/{id}/update";
            var body = ModelSerializer.Serialize<UpdateTestItemRequest>(model);
            var response = await _httpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json"));
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Deletes specified test item.
        /// </summary>
        /// <param name="id">ID of the test item to delete.</param>
        /// <returns>A message from service.</returns>
        public async Task<Message> DeleteTestItemAsync(string id)
        {
            var uri = $"{Project}/item/{id}";
            var response = await _httpClient.DeleteAsync(uri);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Assign issues to specified test items.
        /// </summary>
        /// <param name="model">Information about test items and their issues.</param>
        /// <returns>A list of assigned issues.</returns>
        public async Task<List<Issue>> AssignTestItemIssuesAsync(AssignTestItemIssuesRequest model)
        {
            var uri = $"{Project}/item";
            var body = ModelSerializer.Serialize<AssignTestItemIssuesRequest>(model);
            var response = await _httpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json"));
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<List<Issue>>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Get the history of test item executions.
        /// </summary>
        /// <param name="testItemId">ID of test item.</param>
        /// <param name="depth">How many executions to return.</param>
        /// <param name="full"></param>
        /// <returns>The list of execution history.</returns>
        public async Task<List<TestItemHistory>> GetTestItemHistoryAsync(string testItemId, int depth, bool full)
        {
            var uri = $"{Project}/item/history?ids={testItemId}&history_depth={depth}&is_full={full}";

            var response = await _httpClient.GetAsync(uri);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<List<TestItemHistory>>(await response.Content.ReadAsStringAsync());
        }
    }
}
