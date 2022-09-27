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
        ValueTask<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request);

        /// <inheritdoc cref="AssignIssuesAsync(AssignTestItemIssuesRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes specified test item.
        /// </summary>
        /// <param name="id">ID of the test item to delete.</param>
        /// <returns>A message from service.</returns>
        ValueTask<MessageResponse> DeleteAsync(long id);

        /// <inheritdoc cref="DeleteAsync(long)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Finishes specified test item.
        /// </summary>
        /// <param name="uuid">ID of specified test item.</param>
        /// <param name="request">Information about representation of test item to finish.</param>
        /// <returns>A message from service.</returns>
        ValueTask<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request);

        /// <inheritdoc cref="FinishAsync(string, FinishTestItemRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Returns specified test item by ID.
        /// </summary>
        /// <param name="id">ID of the test item to retrieve.</param>
        /// <returns>A representation of test item.</returns>
        ValueTask<TestItemResponse> GetAsync(long id);

        /// <inheritdoc cref="GetAsync(long)"/>
        /// <param name="cancellationToken">Cancellation token</param>
        ValueTask<TestItemResponse> GetAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns specified test item by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the test item to retrieve.</param>
        /// <returns>A representation of test item.</returns>
        ValueTask<TestItemResponse> GetAsync(string uuid);

        /// <inheritdoc cref="GetAsync(string)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<TestItemResponse> GetAsync(string uuid, CancellationToken cancellationToken);

        /// <summary>
        /// Get the history of test items executions.
        /// </summary>
        /// <param name="id">ID of test item.</param>
        /// <param name="depth">How many executions to return.</param>
        /// <returns>The list of execution history.</returns>
        ValueTask<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth);

        /// <inheritdoc cref="GetHistoryAsync(long, int)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a list of test items for specified launch and parent test item (optional).
        /// </summary>
        /// <returns>A list of test items.</returns>
        ValueTask<Content<TestItemResponse>> GetAsync();

        /// <inheritdoc cref=" GetAsync()"/>
        /// <param name="filterOption">Specified criterias for retrieving test items.</param>
        ValueTask<Content<TestItemResponse>> GetAsync(FilterOption filterOption);

        /// <inheritdoc cref="GetAsync()"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<Content<TestItemResponse>> GetAsync(CancellationToken cancellationToken);

        /// <inheritdoc cref="GetAsync(FilterOption)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<Content<TestItemResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="request">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        ValueTask<TestItemCreatedResponse> StartAsync(StartTestItemRequest request);

        /// <inheritdoc cref="StartAsync(StartTestItemRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<TestItemCreatedResponse> StartAsync(StartTestItemRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new test item.
        /// </summary>
        /// <param name="uuid">UUID of parent item.</param>
        /// <param name="model">Information about representation of test item.</param>
        /// <returns>Representation of created test item.</returns>
        ValueTask<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model);

        /// <inheritdoc cref="StartAsync(string, StartTestItemRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model, CancellationToken cancellationToken);

        /// <summary>
        /// Update specified test item.
        /// </summary>
        /// <param name="id">ID of test item to update.</param>
        /// <param name="request">Information about test item.</param>
        /// <returns>A message from service.</returns>
        ValueTask<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request);

        /// <inheritdoc cref="UpdateAsync(long, UpdateTestItemRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        ValueTask<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request, CancellationToken cancellationToken);
    }
}
