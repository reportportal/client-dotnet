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
        /// Asynchronously finishes specified test item.
        /// </summary>
        /// <param name="uuid">ID of specified test item.</param>
        /// <param name="request">Information about representation of test item to finish.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously creates a new test item.
        /// </summary>
        /// <param name="request">Information about representation of test item.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Representation of created test item.</returns>
        Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously creates a new test item.
        /// </summary>
        /// <param name="uuid">UUID of parent item.</param>
        /// <param name="model">Information about representation of test item.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Representation of created test item.</returns>
        Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model, CancellationToken cancellationToken = default);
    }
}
