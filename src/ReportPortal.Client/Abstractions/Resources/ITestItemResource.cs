using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    /// <summary>
    /// Interacts with test items.
    /// </summary>
    public interface ITestItemResource
    {
        /// <summary>
        /// Assign issues to specified test items.
        /// </summary>
        /// <param name="request">Information about test items and their issues.</param>
        /// <returns>A list of assigned issues.</returns>
        Task<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request);

        /// <summary>
        /// Assign issues to specified test items.
        /// </summary>
        /// <param name="request">Information about test items and their issues.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of assigned issues.</returns>
        Task<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes specified test item.
        /// </summary>
        /// <param name="id">ID of the test item to delete.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> DeleteAsync(long id);

        /// <summary>
        /// Deletes specified test item.
        /// </summary>
        /// <param name="id">ID of the test item to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Finishes specified test item.
        /// </summary>
        /// <param name="uuid">ID of specified test item.</param>
        /// <param name="request">Information about representation of test item to finish.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request);

        /// <summary>
        /// Finishes specified test item.
        /// </summary>
        /// <param name="uuid">ID of specified test item.</param>
        /// <param name="request">Information about representation of test item to finish.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Returns specified test item by ID.
        /// </summary>
        /// <param name="id">ID of the test item to retrieve.</param>
        /// <returns>A representation of test item.</returns>
        Task<TestItemResponse> GetAsync(long id);

        /// <summary>
        /// Returns specified test item by ID.
        /// </summary>
        /// <param name="id">ID of the test item to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representation of test item.</returns>
        Task<TestItemResponse> GetAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns specified test item by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the test item to retrieve.</param>
        /// <returns>A representation of test item.</returns>
        Task<TestItemResponse> GetAsync(string uuid);

        /// <summary>
        /// Returns specified test item by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the test item to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representation of test item.</returns>
        Task<TestItemResponse> GetAsync(string uuid, CancellationToken cancellationToken);

        /// <summary>
        /// Get the history of test items executions.
        /// </summary>
        /// <param name="id">ID of test item.</param>
        /// <param name="depth">How many executions to return.</param>
        /// <returns>The list of execution history.</returns>
        Task<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth);

        /// <summary>
        /// Get the history of test items executions.
        /// </summary>
        /// <param name="id">ID of test item.</param>
        /// <param name="depth">How many executions to return.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The list of execution history.</returns>
        Task<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a list of test items for specified launch and parent test item (optional).
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving test items.</param>
        /// <returns>A list of test items.</returns>
        Task<Content<TestItemResponse>> GetAsync(FilterOption filterOption = null);

        /// <summary>
        /// Returns a list of test items for specified launch and parent test item (optional).
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving test items.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of test items.</returns>
        Task<Content<TestItemResponse>> GetAsync(CancellationToken cancellationToken, FilterOption filterOption = null);

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="request">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request);

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="request">Information about representation of test item.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Representation of created test item.</returns>
        Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="uuid">UUID of parent item.</param>
        /// <param name="model">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model);

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="uuid">UUID of parent item.</param>
        /// <param name="model">Information about representation of test item.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Representation of created test item.</returns>
        Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model, CancellationToken cancellationToken);

        /// <summary>
        /// Update specified test item.
        /// </summary>
        /// <param name="id">ID of test item to update.</param>
        /// <param name="request">Information about test item.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request);

        /// <summary>
        /// Update specified test item.
        /// </summary>
        /// <param name="id">ID of test item to update.</param>
        /// <param name="request">Information about test item.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request, CancellationToken cancellationToken);
    }
}
