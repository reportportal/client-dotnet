using System.Collections.Generic;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace ReportPortal.Client
{
    public partial class Service
    {
        /// <summary>
        /// Returns a list of test items for specified launch and parent test item (optional).
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving test items.</param>
        /// <returns>A list of test items.</returns>
        public TestItemsContainer GetTestItems(FilterOption filterOption = null)
        {
            var request = new RestRequest("item");
            if (filterOption != null)
            {
                foreach (var p in filterOption.ConvertToDictionary())
                {
                    request.AddParameter(p.Key, p.Value, ParameterType.QueryString);
                }
            }
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<TestItemsContainer>(JObject.Parse(response.Content).ToString());
        }

        /// <summary>
        /// Returns specified test item by ID.
        /// </summary>
        /// <param name="id">ID of the test item to retrieve.</param>
        /// <returns>A representation of test item.</returns>
        public TestItem GetTestItem(string id)
        {
            var request = new RestRequest("item/" + id);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<TestItem>(response.Content);
        }

        /// <summary>
        /// Returns the list of tests tags for specified launch.
        /// </summary>
        /// <param name="launchId">ID of launch.</param>
        /// <param name="tagContains">Tags should contain specified text.</param>
        /// <returns></returns>
        public List<string> GetUniqueTags(string launchId, string tagContains)
        {
            var request = new RestRequest("item/tags");
            request.AddParameter("launch", launchId, ParameterType.QueryString);
            request.AddParameter("filter.cnt.tags", tagContains, ParameterType.QueryString);

            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<List<string>>(response.Content);
        }

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="model">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        public TestItem StartTestItem(StartTestItemRequest model)
        {
            var request = new RestRequest("item", Method.POST);
            var body = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<TestItem>(response.Content);
        }

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="id">ID of parent item.</param>
        /// <param name="model">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        public TestItem StartTestItem(string id, StartTestItemRequest model)
        {
            var request = new RestRequest("item/" + id, Method.POST);
            var body = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<TestItem>(response.Content);
        }

        /// <summary>
        /// Finishes specified test item.
        /// </summary>
        /// <param name="id">ID of specified test item.</param>
        /// <param name="model">Information about representation of test item to finish.</param>
        /// <returns>A message from service.</returns>
        public Message FinishTestItem(string id, FinishTestItemRequest model)
        {
            var request = new RestRequest("item/" + id, Method.PUT);
            var body = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<Message>(response.Content);
        }

        /// <summary>
        /// Update specified test item.
        /// </summary>
        /// <param name="id">ID of test item to update.</param>
        /// <param name="model">Information about test item.</param>
        /// <returns>A message from service.</returns>
        public Message UpdateTestItem(string id, UpdateTestItemRequest model)
        {
            var request = new RestRequest("item/" + id + "/update", Method.PUT);
            var body = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<Message>(response.Content);
        }

        /// <summary>
        /// Deletes specified test item.
        /// </summary>
        /// <param name="id">ID of the test item to delete.</param>
        /// <returns>A message from service.</returns>
        public Message DeleteTestItem(string id)
        {
            var request = new RestRequest("item/" + id, Method.DELETE);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<Message>(response.Content);
        }

        /// <summary>
        /// Assign issues to specified test items.
        /// </summary>
        /// <param name="model">Information about test items and their issues.</param>
        /// <returns>A list of assigned issues.</returns>
        public List<Issue> AssignTestItemIssues(AssignTestItemIssuesRequest model)
        {
            var request = new RestRequest("item", Method.PUT);
            var body = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<List<Issue>>(response.Content);
        }

        /// <summary>
        /// Get the history of test item executions.
        /// </summary>
        /// <param name="testItemId">ID of test item.</param>
        /// <param name="depth">How many executions to return.</param>
        /// <param name="full"></param>
        /// <returns>The list of execution history.</returns>
        public List<TestItemHistory> GetTestItemHistory(string testItemId, int depth, bool full)
        {
            var request = new RestRequest("item/history", Method.GET);
            request.AddParameter("ids", testItemId, ParameterType.QueryString);
            request.AddParameter("history_depth", depth, ParameterType.QueryString);
            request.AddParameter("is_full", full, ParameterType.QueryString);
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<List<TestItemHistory>>(response.Content);
        }
    }
}
