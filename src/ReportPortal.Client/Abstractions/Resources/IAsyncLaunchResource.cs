using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    /// <summary>
    /// Asynchronously interacts with launches.
    /// </summary>
    public interface IAsyncLaunchResource
    {
        /// <summary>
        /// Asynchronously finishes a specified launch.
        /// </summary>
        /// <param name="uuid">The UUID of the specified launch.</param>
        /// <param name="request">Information about the representation of the launch to finish.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A message from the service.</returns>
        Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously merges several launches.
        /// </summary>
        /// <param name="request">The request for merging.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the model of the merged launches.</returns>
        Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously starts a new launch.
        /// </summary>
        /// <param name="request">Information about the launch.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Information about the started launch.</returns>
        Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request, CancellationToken cancellationToken = default);
    }
}
