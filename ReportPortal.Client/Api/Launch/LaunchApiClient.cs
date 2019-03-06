using System;
using System.Threading.Tasks;
using ReportPortal.Client.Converter;
using System.Net.Http;
using System.Text;
using ReportPortal.Client.Extention;
using ReportPortal.Client.Api.Launch.Requests;
using ReportPortal.Client.Common.Model;
using ReportPortal.Client.Api.Launch.Model;
using ReportPortal.Client.Common.Model.Paging;
using ReportPortal.Client.Common.Model.Filtering;

namespace ReportPortal.Client.Api.Launch
{
    public class LaunchApiClient : BaseApiClient, ILaunchApiClient
    {
        public LaunchApiClient(HttpClient httpCLient, Uri baseUri, string project) : base(httpCLient, baseUri, project)
        {
        }

        public virtual async Task<PagingContent<LaunchModel>> GetLaunchesAsync(FilterOption filterOption = null, bool debug = false)
        {
            var uri = BaseUri.Append($"{Project}/launch");
            if (debug) { uri = uri.Append("mode"); }

            if (filterOption != null)
            {
                uri = uri.Append($"?{filterOption}");
            }

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<PagingContent<LaunchModel>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Model.LaunchModel> GetLaunchAsync(string id)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Model.LaunchModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Model.LaunchModel> StartLaunchAsync(StartLaunchRequest startLaunchRequest)
        {
            var uri = BaseUri.Append($"{Project}/launch");
            var body = ModelSerializer.Serialize<StartLaunchRequest>(startLaunchRequest);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Model.LaunchModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Message> FinishLaunchAsync(string id, FinishLaunchRequest finishLaunchRequest, bool force = false)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}");
            uri = force == true ? uri.Append("/stop") : uri.Append("/finish");
            var body = ModelSerializer.Serialize<FinishLaunchRequest>(finishLaunchRequest);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Message> DeleteLaunchAsync(string id)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}");
            var response = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Model.LaunchModel> MergeLaunchesAsync(MergeLaunchesRequest mergeLaunchesRequest)
        {
            var uri = BaseUri.Append($"{Project}/launch/merge");
            var body = ModelSerializer.Serialize<MergeLaunchesRequest>(mergeLaunchesRequest);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Model.LaunchModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Message> UpdateLaunchAsync(string id, UpdateLaunchRequest updateLaunchRequest)
        {
            var uri = BaseUri.Append($"{Project}/launch/{id}/update");
            var body = ModelSerializer.Serialize<UpdateLaunchRequest>(updateLaunchRequest);
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Message> AnalyzeLaunchAsync(AnalyzeLaunchRequest analyzeLaunchRequest)
        {
            var uri = BaseUri.Append($"{Project}/launch/analyze");
            var body = ModelSerializer.Serialize<AnalyzeLaunchRequest>(analyzeLaunchRequest);
            var response = await HttpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
