using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions
{
    public interface ILaunchResource
    {
        Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest model);
        Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest model);
        Task<MessageResponse> DeleteAsync(long id);
        Task<LaunchResponse> GetAsync(long id);
        Task<LaunchResponse> GetAsync(string uuid);
        Task<Content<LaunchResponse>> GetAsync(FilterOption filterOption = null, bool debug = false);
        Task<LaunchResponse> MergeAsync(MergeLaunchesRequest model);
        Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request);
        Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest model);
        Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest model);
    }
}
