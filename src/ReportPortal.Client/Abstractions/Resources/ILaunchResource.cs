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
        /// <returns>A message from service.</returns>
        Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request);

        /// <inheritdoc cref="AnalyzeAsync(AnalyzeLaunchRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Finishes specified launch.
        /// </summary>
        /// <param name="uuid">UUID of specified launch.</param>
        /// <param name="request">Information about representation of launch to finish.</param>
        /// <returns>A message from service.</returns>
        Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request);

        /// <inheritdoc cref="FinishAsync(string, FinishLaunchRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes specified launch.
        /// </summary>
        /// <param name="id">ID of the launch to delete.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> DeleteAsync(long id);

        /// <inheritdoc cref="DeleteAsync(long)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns specified launch by ID.
        /// </summary>
        /// <param name="id">ID of the launch to retrieve.</param>
        /// <returns>A representation of launch.</returns>
        Task<LaunchResponse> GetAsync(long id);

        /// <inheritdoc cref="GetAsync(long)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LaunchResponse> GetAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns specified launch by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the launch to retrieve.</param>
        /// <returns>A representation of launch.</returns>
        Task<LaunchResponse> GetAsync(string uuid);

        /// <inheritdoc cref="GetAsync(long)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LaunchResponse> GetAsync(string uuid, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a list of launches for current project.
        /// </summary>
        /// <returns>A list of launches.</returns>
        Task<Content<LaunchResponse>> GetAsync();

        /// <inheritdoc cref="GetAsync()"/>
        /// <param name="filterOption">Specified criterias for retrieving launches.</param>
        Task<Content<LaunchResponse>> GetAsync(FilterOption filterOption);

        /// <inheritdoc cref="GetAsync()"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<Content<LaunchResponse>> GetAsync(CancellationToken cancellationToken);

        /// <inheritdoc cref="GetAsync(FilterOption)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<Content<LaunchResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a list of debug launches for current project.
        /// </summary>
        /// <returns>A list of launches.</returns>
        Task<Content<LaunchResponse>> GetDebugAsync();

        /// <inheritdoc cref="GetDebugAsync()"/>
        /// <param name="filterOption">Specified criterias for retrieving launches.</param>
        Task<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption);

        /// <inheritdoc cref="GetDebugAsync()"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<Content<LaunchResponse>> GetDebugAsync(CancellationToken cancellationToken);

        /// <inheritdoc cref="GetDebugAsync(FilterOption)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption, CancellationToken cancellationToken);

        /// <summary>
        /// Merge several launches.
        /// </summary>
        /// <param name="request">Request for merging.</param>
        /// <returns>Returns the model of merged launches.</returns>
        Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request);

        /// <inheritdoc cref="MergeAsync(MergeLaunchesRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Starts new launch.
        /// </summary>
        /// <param name="request">Information about launch.</param>
        /// <returns>Information about started launch.</returns>
        Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request);

        /// <inheritdoc cref="StartAsync(StartLaunchRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Stopes specified launch even if inner tests are not finished yet.
        /// </summary>
        /// <param name="id">ID of specified launch.</param>
        /// <param name="request">Information about representation of launch to finish.</param>
        /// <returns>A message from service.</returns>
        Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request);

        /// <inheritdoc cref="StopAsync(long, FinishLaunchRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Update specified launch.
        /// </summary>
        /// <param name="id">ID of launch to update.</param>
        /// <param name="request">Information about launch.</param>
        /// <returns>A message from service.</returns>
        Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request);

        /// <inheritdoc cref="UpdateAsync(long, UpdateLaunchRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request, CancellationToken cancellationToken);
    }
}
