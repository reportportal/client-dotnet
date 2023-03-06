﻿using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceLaunchResource : ServiceBaseResource, ILaunchResource
    {
        public ServiceLaunchResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public Task<LaunchResponse> GetAsync(string uuid)
        {
            return GetAsync(uuid, CancellationToken.None);
        }

        public async Task<LaunchResponse> GetAsync(string uuid, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<LaunchResponse>($"v1/{ProjectName}/launch/uuid/{uuid}", cancellationToken).ConfigureAwait(false);
        }

        public Task<LaunchResponse> GetAsync(long id)
        {
            return GetAsync(id, CancellationToken.None);
        }

        public async Task<LaunchResponse> GetAsync(long id, CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<LaunchResponse>($"v1/{ProjectName}/launch/{id}", cancellationToken).ConfigureAwait(false);
        }

        public Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request)
        {
            return StartAsync(request, CancellationToken.None);
        }

        public async Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<LaunchCreatedResponse, StartLaunchRequest>(
                $"v1/{ProjectName}/launch", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request)
        {
            return FinishAsync(uuid, request, CancellationToken.None);
        }

        public async Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<LaunchFinishedResponse, FinishLaunchRequest>(
                $"v1/{ProjectName}/launch/{uuid}/finish", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request)
        {
            return StopAsync(id, request, CancellationToken.None);
        }

        public async Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<LaunchFinishedResponse, FinishLaunchRequest>(
                $"v1/{ProjectName}/launch/{id}/stop", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<MessageResponse> DeleteAsync(long id)
        {
            return DeleteAsync(id, CancellationToken.None);
        }

        public async Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            return await DeleteAsJsonAsync<MessageResponse>($"v1/{ProjectName}/launch/{id}", cancellationToken).ConfigureAwait(false);
        }

        public Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request)
        {
            return MergeAsync(request, CancellationToken.None);
        }

        public async Task<LaunchResponse> MergeAsync(MergeLaunchesRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<LaunchResponse, MergeLaunchesRequest>(
                $"v1/{ProjectName}/launch/merge", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request)
        {
            return UpdateAsync(id, request, CancellationToken.None);
        }

        public async Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PutAsJsonAsync<MessageResponse, UpdateLaunchRequest>(
                $"v1/{ProjectName}/launch/{id}/update", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request)
        {
            return AnalyzeAsync(request, CancellationToken.None);
        }

        public async Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request, CancellationToken cancellationToken)
        {
            return await PostAsJsonAsync<MessageResponse, AnalyzeLaunchRequest>(
                $"v1/{ProjectName}/launch/analyze", request, cancellationToken).ConfigureAwait(false);
        }

        public Task<Content<LaunchResponse>> GetAsync()
        {
            return GetAsync(filterOption: null, CancellationToken.None);
        }
        
        public Task<Content<LaunchResponse>> GetAsync(FilterOption filterOption)
        {
            return GetAsync(filterOption, CancellationToken.None);
        }

        public async Task<Content<LaunchResponse>> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync(filterOption: null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Content<LaunchResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            var uri = $"v1/{ProjectName}/launch";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<LaunchResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }

        public Task<Content<LaunchResponse>> GetDebugAsync()
        {
            return GetDebugAsync(filterOption: null, CancellationToken.None);
        }

        public Task<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption)
        {
            return GetDebugAsync(filterOption, CancellationToken.None);
        }

        public async Task<Content<LaunchResponse>> GetDebugAsync(CancellationToken cancellationToken)
        {
            return await GetDebugAsync(filterOption: null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            var uri = $"v1/{ProjectName}/launch/mode";

            if (filterOption != null)
            {
                uri += $"?{filterOption}";
            }

            return await GetAsJsonAsync<Content<LaunchResponse>>(uri, cancellationToken).ConfigureAwait(false);
        }
    }
}
