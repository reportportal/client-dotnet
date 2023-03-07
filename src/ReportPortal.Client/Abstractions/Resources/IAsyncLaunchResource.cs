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
        /// Asynchronously finishes specified launch.
        /// </summary>
        /// <param name="uuid">UUID of specified launch.</param>
        /// <param name="request">Information about representation of launch to finish.</param>
        /// <returns>A message from service.</returns>
        Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request);

        /// <inheritdoc cref="FinishAsync(string, FinishLaunchRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously merge several launches.
        /// </summary>
        /// <param name="request">Request for merging.</param>
        /// <returns>Returns the model of merged launches.</returns>
        Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request);

        /// <inheritdoc cref="MergeAsync(MergeLaunchesRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously starts new launch.
        /// </summary>
        /// <param name="request">Information about launch.</param>
        /// <returns>Information about started launch.</returns>
        Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request);

        /// <inheritdoc cref="StartAsync(StartLaunchRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request, CancellationToken cancellationToken);
    }
}
