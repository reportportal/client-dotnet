//using System.Collections.Generic;
//using System.Threading.Tasks;
//using ReportPortal.Client.Extentions;
//using ReportPortal.Client.Filtering;
//using ReportPortal.Client.Models;
//using ReportPortal.Client.Requests;
//using RestSharp;
//using ReportPortal.Client.Converters;

//namespace ReportPortal.Client
//{
//    public partial class Service
//    {
//        /// <summary>
//        /// Returns a list of test items for specified launch and parent test item (optional).
//        /// </summary>
//        /// <param name="filterOption">Specified criterias for retrieving test items.</param>
//        /// <returns>A list of test items.</returns>
//        public TestItemsContainer GetTestItems(FilterOption filterOption = null)
//        {
//            var request = new RestRequest(Project + "/item");
//            if (filterOption != null)
//            {
//                foreach (var p in filterOption.ConvertToDictionary())
//                {
//                    request.AddParameter(p.Key, p.Value, ParameterType.QueryString);
//                }
//            }
//            var response = _restClient.ExecuteWithErrorHandling(request);
//            return ModelSerializer.Deserialize<TestItemsContainer>(response.Content);
//        }

//        /// <summary>
//        /// Returns specified test item by ID.
//        /// </summary>
//        /// <param name="id">ID of the test item to retrieve.</param>
//        /// <returns>A representation of test item.</returns>
//        public TestItem GetTestItem(string id)
//        {
//            var request = new RestRequest(Project + "/item/" + id);
//            var response = _restClient.ExecuteWithErrorHandling(request);
//            return ModelSerializer.Deserialize<TestItem>(response.Content);
//        }

//        /// <summary>
//        /// Returns the list of tests tags for specified launch.
//        /// </summary>
//        /// <param name="launchId">ID of launch.</param>
//        /// <param name="tagContains">Tags should contain specified text.</param>
//        /// <returns></returns>
//        public List<string> GetUniqueTags(string launchId, string tagContains)
//        {
//            var request = new RestRequest(Project + "/item/tags");
//            request.AddParameter("launch", launchId, ParameterType.QueryString);
//            request.AddParameter("filter.cnt.tags", tagContains, ParameterType.QueryString);

//            var response = _restClient.ExecuteWithErrorHandling(request);
//            return ModelSerializer.Deserialize<List<string>>(response.Content);
//        }

//        /// <summary>
//        /// Creates a new test item.
//        /// </summary>
//        /// <param name="model">Information about representation of test item.</param>
//        /// <returns>Representation of created test item.</returns>
//        public TestItem StartTestItem(StartTestItemRequest model)
//        {
//            var request = new RestRequest(Project + "/item", Method.POST);
//            var body = ModelSerializer.Serialize<StartTestItemRequest>(model);
//            request.AddParameter("application/json", body, ParameterType.RequestBody);
//            var response = _restClient.ExecuteWithErrorHandling(request);
//            return ModelSerializer.Deserialize<TestItem>(response.Content);
//        }

//        public async Task<TestItem> StartTestItemAsync(StartTestItemRequest model)
//        {
//            return await Task.Run(() => StartTestItem(model));
//        }

//        /// <summary>
//        /// Creates a new test item.
//        /// </summary>
//        /// <param name="id">ID of parent item.</param>
//        /// <param name="model">Information about representation of test item.</param>
//        /// <returns>Representation of created test item.</returns>
//        public TestItem StartTestItem(string id, StartTestItemRequest model)
//        {
//            var request = new RestRequest(Project + "/item/" + id, Method.POST);
//            var body = ModelSerializer.Serialize<StartTestItemRequest>(model);
//            request.AddParameter("application/json", body, ParameterType.RequestBody);
//            var response = _restClient.ExecuteWithErrorHandling(request);
//            return ModelSerializer.Deserialize<TestItem>(response.Content);
//        }

//        public async Task<TestItem> StartTestItemAsync(string id, StartTestItemRequest model)
//        {
//            return await Task.Run(() => StartTestItem(id, model));
//        }

//        /// <summary>
//        /// Finishes specified test item.
//        /// </summary>
//        /// <param name="id">ID of specified test item.</param>
//        /// <param name="model">Information about representation of test item to finish.</param>
//        /// <returns>A message from service.</returns>
//        public Message FinishTestItem(string id, FinishTestItemRequest model)
//        {
//            var request = new RestRequest(Project + "/item/" + id, Method.PUT);
//            var body = ModelSerializer.Serialize<FinishTestItemRequest>(model);
//            request.AddParameter("application/json", body, ParameterType.RequestBody);
//            var response = _restClient.ExecuteWithErrorHandling(request);
//            return ModelSerializer.Deserialize<Message>(response.Content);
//        }

//        public async Task<Message> FinishTestItemAsync(string id, FinishTestItemRequest model)
//        {
//            return await Task.Run(() => FinishTestItem(id, model));
//        }

//        /// <summary>
//        /// Update specified test item.
//        /// </summary>
//        /// <param name="id">ID of test item to update.</param>
//        /// <param name="model">Information about test item.</param>
//        /// <returns>A message from service.</returns>
//        public Message UpdateTestItem(string id, UpdateTestItemRequest model)
//        {
//            var request = new RestRequest(Project + "/item/" + id + "/update", Method.PUT);
//            var body = ModelSerializer.Serialize<UpdateTestItemRequest>(model);
//            request.AddParameter("application/json", body, ParameterType.RequestBody);
//            var response = _restClient.ExecuteWithErrorHandling(request);
//            return ModelSerializer.Deserialize<Message>(response.Content);
//        }

//        public async Task<Message> UpdateTestItemAsync(string id, UpdateTestItemRequest model)
//        {
//            return await Task.Run(() => UpdateTestItem(id, model));
//        }

//        /// <summary>
//        /// Deletes specified test item.
//        /// </summary>
//        /// <param name="id">ID of the test item to delete.</param>
//        /// <returns>A message from service.</returns>
//        public Message DeleteTestItem(string id)
//        {
//            var request = new RestRequest(Project + "/item/" + id, Method.DELETE);
//            var response = _restClient.ExecuteWithErrorHandling(request);
//            return ModelSerializer.Deserialize<Message>(response.Content);
//        }

//        /// <summary>
//        /// Assign issues to specified test items.
//        /// </summary>
//        /// <param name="model">Information about test items and their issues.</param>
//        /// <returns>A list of assigned issues.</returns>
//        public List<Issue> AssignTestItemIssues(AssignTestItemIssuesRequest model)
//        {
//            var request = new RestRequest(Project + "/item", Method.PUT);
//            var body = ModelSerializer.Serialize<AssignTestItemIssuesRequest>(model);
//            request.AddParameter("application/json", body, ParameterType.RequestBody);
//            var response = _restClient.ExecuteWithErrorHandling(request);
//            return ModelSerializer.Deserialize<List<Issue>>(response.Content);
//        }

//        /// <summary>
//        /// Get the history of test item executions.
//        /// </summary>
//        /// <param name="testItemId">ID of test item.</param>
//        /// <param name="depth">How many executions to return.</param>
//        /// <param name="full"></param>
//        /// <returns>The list of execution history.</returns>
//        public List<TestItemHistory> GetTestItemHistory(string testItemId, int depth, bool full)
//        {
//            var request = new RestRequest(Project + "/item/history", Method.GET);
//            request.AddParameter("ids", testItemId, ParameterType.QueryString);
//            request.AddParameter("history_depth", depth, ParameterType.QueryString);
//            request.AddParameter("is_full", full, ParameterType.QueryString);
//            var response = _restClient.ExecuteWithErrorHandling(request);
//            return ModelSerializer.Deserialize<List<TestItemHistory>>(response.Content);
//        }
//    }
//}
