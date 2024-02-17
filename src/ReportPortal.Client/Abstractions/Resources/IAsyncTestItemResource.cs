using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    /// <summary>
    /// Asynchronously interacts with test items.
    /// </summary>
    public interface IAsyncTestItemResource
    {
        /// <summary>
        /// Asynchronously finishes a specified test item.
        /// </summary>
        /// <param name="uuid">The ID of the specified test item.</param>
        /// <param name="request">Information about the representation of the test item to finish.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A message from the service.</returns>
        Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously creates a new test item.
        /// </summary>
        /// <param name="request">Information about the representation of the test item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The representation of the created test item.</returns>
        Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously creates a new test item.
        /// </summary>
        /// <param name="uuid">The UUID of the parent item.</param>
        /// <param name="model">Information about the representation of the test item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The representation of the created test item.</returns>
        Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model, CancellationToken cancellationToken = default);
    }
}
