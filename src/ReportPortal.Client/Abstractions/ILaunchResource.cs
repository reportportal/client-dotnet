using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions
{
    public interface ILaunchResource
    {
        Task<Message> AnalyzeAsync(AnalyzeLaunchRequest model);
        Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest model);
        Task<Message> DeleteAsync(long id);
        Task<LaunchResponse> GetAsync(long id);
        Task<LaunchResponse> GetAsync(string uuid);
        Task<LaunchesContainer> GetAsync(FilterOption filterOption = null, bool debug = false);
        Task<LaunchResponse> MergeAsync(MergeLaunchesRequest model);
        Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request);
        Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest model);
        Task<Message> UpdateAsync(long id, UpdateLaunchRequest model);
    }
}
