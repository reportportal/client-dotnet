using ReportPortal.Client.Api.TestItem.Model;
using ReportPortal.Client.Api.TestItem.Request;
using ReportPortal.Client.Common.Model;
using ReportPortal.Client.Common.Model.Filtering;
using ReportPortal.Client.Common.Model.Paging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Client.Api.TestItem
{
    public interface ITestItemApiClient
    {
        /// <summary>
        /// Returns a list of test items for the launch.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving test items.</param>
        /// <returns>A list of test items.</returns>
        Task<PagingContent<TestItemModel>> GetTestItemsAsync(FilterOption filterOption = null);

        /// <summary>
        /// Returns specified test item by ID.
        /// </summary>
        /// <param name="id">ID of the test item to retrieve.</param>
        /// <returns>A representation of test item.</returns>
        Task<TestItemModel> GetTestItemAsync(string id);

        /// <summary>
        /// Returns the list of tests tags for specified launch.
        /// </summary>
        /// <param name="launchId">ID of launch.</param>
        /// <param name="tagContains">Tags should contain specified text.</param>
        /// <returns></returns>
        Task<List<string>> GetUniqueTagsAsync(string launchId, string tagContains);

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="model">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        Task<TestItemModel> StartTestItemAsync(StartTestItemRequest model);

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="id">ID of parent item.</param>
        /// <param name="model">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        Task<TestItemModel> StartTestItemAsync(string id, StartTestItemRequest model);

        /// <summary>
        /// Finishes specified test item.
        /// </summary>
        /// <param name="id">ID of specified test item.</param>
        /// <param name="model">Information about representation of test item to finish.</param>
        /// <returns>A message from service.</returns>
        Task<Message> FinishTestItemAsync(string id, FinishTestItemRequest model);

        /// <summary>
        /// Update specified test item.
        /// </summary>
        /// <param name="id">ID of test item to update.</param>
        /// <param name="model">Information about test item.</param>
        /// <returns>A message from service.</returns>
        Task<Message> UpdateTestItemAsync(string id, UpdateTestItemRequest model);

        /// <summary>
        /// Deletes specified test item.
        /// </summary>
        /// <param name="id">ID of the test item to delete.</param>
        /// <returns>A message from service.</returns>
        Task<Message> DeleteTestItemAsync(string id);

        /// <summary>
        /// Assign issues to specified test items.
        /// </summary>
        /// <param name="model">Information about test items and their issues.</param>
        /// <returns>A list of assigned issues.</returns>
        Task<List<Issue>> AssignTestItemIssuesAsync(AssignTestItemIssuesRequest model);

        /// <summary>
        /// Get the history of test item executions.
        /// </summary>
        /// <param name="testItemId">ID of test item.</param>
        /// <param name="depth">How many executions to return.</param>
        /// <param name="full"></param>
        /// <returns>The list of execution history.</returns>
        Task<List<TestItemHistoryModel>> GetTestItemHistoryAsync(string testItemId, int depth, bool full);
    }
}
