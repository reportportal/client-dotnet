using System;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Converters;
using System.Net.Http;
using System.Text;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Filtering;

namespace ReportPortal.Client
{
    public class ServiceLaunchResource : BaseResource, ILaunchResource
    {
        public ServiceLaunchResource(HttpClient httpClient, Uri baseUri, string project, string token) : base(httpClient, baseUri, project, token)
        {

        }

        /// <summary>
        /// Returns a list of launches for current project.
        /// </summary>
        /// <param name="filterOption">Specified criterias for retrieving launches.</param>
        /// <param name="debug">Returns user debug launches or not.</param>
        /// <returns>A list of launches.</returns>
        public virtual async Task<Content<LaunchResponse>> GetAsync(FilterOption filterOption = null, bool debug = false)
        {
            var uri = BaseUri.Append($"{Project}/launch");
            if (debug) { uri = uri.Append("mode"); }

            if (filterOption != null)
            {
                uri = uri.Append($"?{filterOption}");
            }

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Content<LaunchResponse>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Returns specified launch by UUID.
        /// </summary>
        /// <param name="uuid">UUID of the launch to retrieve.</param>
        /// <returns>A representation of launch.</returns>
        public virtual async Task<LaunchResponse> GetAsync(string uuid)
        {
            var uri = BaseUri.Append($"{Project}/launch/uuid/{uuid}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Returns specified launch by ID.
        /// </summary>
        /// <param name="uuid">ID of the launch to retrieve.</param>
        /// <returns>A representation of launch.</returns>
        public virtual async Task<LaunchResponse> GetAsync(long id)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Creates a new launch.
        /// </summary>
        /// <param name="request">Information about representation of launch.</param>
        /// <returns>Representation of just created launch.</returns>
        public virtual async Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request)
        {
            var uri = BaseUri.Append($"{Project}/launch");
            var body = ModelSerializer.Serialize<StartLaunchRequest>(request);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchCreatedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Finishes specified launch.
        /// </summary>
        /// <param name="uuid">UUID of specified launch.</param>
        /// <param name="model">Information about representation of launch to finish.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest model)
        {
            var uri = BaseUri.Append($"{Project}/launch/{uuid}/finish");
            var body = ModelSerializer.Serialize<FinishLaunchRequest>(model);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchFinishedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Stopes specified launch even if inner tests are not finished yet.
        /// </summary>
        /// <param name="id">ID of specified launch.</param>
        /// <param name="model">Information about representation of launch to finish.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest model)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}/stop");
            var body = ModelSerializer.Serialize<FinishLaunchRequest>(model);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchFinishedResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Deletes specified launch.
        /// </summary>
        /// <param name="id">ID of the launch to delete.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<MessageResponse> DeleteAsync(long id)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}");
            var response = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Merge several launches.
        /// </summary>
        /// <param name="model">Request for merging.</param>
        /// <returns>Returns the model of merged launches.</returns>
        public virtual async Task<LaunchResponse> MergeAsync(MergeLaunchesRequest model)
        {
            var uri = BaseUri.Append($"{Project}/launch/merge");
            var body = ModelSerializer.Serialize<MergeLaunchesRequest>(model);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<LaunchResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Update specified launch.
        /// </summary>
        /// <param name="id">ID of launch to update.</param>
        /// <param name="model">Information about launch.</param>
        /// <returns>A message from service.</returns>
        public virtual async Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest model)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}/update");
            var body = ModelSerializer.Serialize<UpdateLaunchRequest>(model);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Merge several launches.
        /// </summary>
        /// <returns>A message from service.</returns>
        public virtual async Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest model)
        {
            var uri = BaseUri.Append($"{Project}/launch/analyze");
            var body = ModelSerializer.Serialize<AnalyzeLaunchRequest>(model);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
