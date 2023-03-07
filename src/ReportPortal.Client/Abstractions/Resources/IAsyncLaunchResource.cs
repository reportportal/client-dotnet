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
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously merge several launches.
        /// </summary>
        /// <param name="request">Request for merging.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Returns the model of merged launches.</returns>
        Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously starts new launch.
        /// </summary>
        /// <param name="request">Information about launch.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Information about started launch.</returns>
        Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request, CancellationToken cancellationToken = default);
    }
}
