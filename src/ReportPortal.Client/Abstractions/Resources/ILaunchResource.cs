using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    /// <summary>
    /// Interacts with launches.
    /// </summary>
    public interface ILaunchResource
    {
        /// <summary>
        /// Analyzes launches.
        /// </summary>
        /// <param name="request">A request how to analyze the launch.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finishes specified launch.
        /// </summary>
        /// <param name="uuid">UUID of specified launch.</param>
        /// <param name="request">Information about representation of launch to finish.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes specified launch.
        /// </summary>
        /// <param name="id">ID of the launch to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns specified launch by ID.
        /// </summary>
        /// <param name="id">ID of the launch to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representation of launch.</returns>
        Task<LaunchResponse> GetAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns specified launch by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the launch to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representation of launch.</returns>
        Task<LaunchResponse> GetAsync(string uuid, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a list of launches for current project.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of launches.</returns>
        Task<Content<LaunchResponse>> GetAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a list of launches for current project.
        /// </summary>
        /// <param name="filterOption">Specified criteria for retrieving launches.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of launches.</returns>
        Task<Content<LaunchResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a list of debug launches for current project.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of launches.</returns>
        Task<Content<LaunchResponse>> GetDebugAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a list of debug launches for current project.
        /// </summary>
        /// <param name="filterOption">Specified criteria for retrieving launches.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of launches.</returns>
        Task<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption, CancellationToken cancellationToken = default);

        /// <summary>
        /// Merge several launches.
        /// </summary>
        /// <param name="request">Request for merging.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Returns the model of merged launches.</returns>
        Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Starts new launch.
        /// </summary>
        /// <param name="request">Information about launch.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Information about started launch.</returns>
        Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops specified launch even if inner tests are not finished yet.
        /// </summary>
        /// <param name="id">ID of specified launch.</param>
        /// <param name="request">Information about representation of launch to finish.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates specified launch.
        /// </summary>
        /// <param name="id">ID of launch to update.</param>
        /// <param name="request">Information about launch.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request, CancellationToken cancellationToken = default);
    }
}
