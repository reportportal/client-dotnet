using System.Threading.Tasks;
using ReportPortal.Client.Api.Launch.Model;
using ReportPortal.Client.Api.Launch.Request;
using ReportPortal.Client.Common.Model;
using ReportPortal.Client.Common.Model.Filtering;
using ReportPortal.Client.Common.Model.Paging;

namespace ReportPortal.Client.Api.Launch
{
    public interface ILaunchApiClient
    {
        /// <summary>
        /// Returns a list of launches for current project.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving launches.</param>
        /// <param name="debug">Returns user debug launches or not.</param>
        /// <returns>A list of launches.</returns>
        Task<PagingContent<LaunchModel>> GetLaunchesAsync(FilterOption filterOption = null, bool debug = false);

        /// <summary>
        /// Returns specified launch by ID.
        /// </summary>
        /// <param name="id">ID of the launch to retrieve.</param>
        /// <returns>A representation of launch.</returns>
        Task<LaunchModel> GetLaunchAsync(string id);

        /// <summary>
        /// Creates a new launch.
        /// </summary>
        /// <param name="startLaunchRequest">Information about representation of launch.</param>
        /// <returns>Representation of just created launch.</returns>
        Task<LaunchModel> StartLaunchAsync(StartLaunchRequest startLaunchRequest);

        /// <summary>
        /// Finishes specified launch.
        /// </summary>
        /// <param name="id">ID of specified launch.</param>
        /// <param name="finishLaunchRequest">Information about representation of launch to finish.</param>
        /// <param name="force">Force finish launch even if test items are in progress.</param>
        /// <returns>A message from service.</returns>
        Task<Message> FinishLaunchAsync(string id, FinishLaunchRequest finishLaunchRequest, bool force = false);

        /// <summary>
        /// Deletes specified launch.
        /// </summary>
        /// <param name="id">ID of the launch to delete.</param>
        /// <returns>A message from service.</returns>
        Task<Message> DeleteLaunchAsync(string id);

        /// <summary>
        /// Merge several launches.
        /// </summary>
        /// <param name="mergeLaunchesRequest">Request for merging.</param>
        /// <returns>Returns the model of merged launches.</returns>
        Task<LaunchModel> MergeLaunchesAsync(MergeLaunchesRequest mergeLaunchesRequest);

        /// <summary>
        /// Update specified launch.
        /// </summary>
        /// <param name="id">ID of launch to update.</param>
        /// <param name="updateLaunchRequest">Information about launch.</param>
        /// <returns>A message from service.</returns>
        Task<Message> UpdateLaunchAsync(string id, UpdateLaunchRequest updateLaunchRequest);

        /// <summary>
        /// Analyze specified launch.
        /// </summary>
        /// <param name="analyzeLaunchRequest">Request for analysis.</param>
        /// <returns>A message from service.</returns>
        Task<Message> AnalyzeLaunchAsync(AnalyzeLaunchRequest analyzeLaunchRequest);
    }
}
