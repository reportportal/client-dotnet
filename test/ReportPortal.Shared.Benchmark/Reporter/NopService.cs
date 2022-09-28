using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Benchmark.Reporter
{
    public class NopService : IClientService
    {
        public ILaunchResource Launch => new NopLaunchResource();

        public ITestItemResource TestItem => new NopTestItemResource();

        public ILogItemResource LogItem => new NopLogItemResourse();

        public IUserResource User => throw new NotImplementedException();

        public IUserFilterResource UserFilter => throw new NotImplementedException();

        public IProjectResource Project => throw new NotImplementedException();
    }

    public class NopLaunchResource : ILaunchResource
    {
        public async ValueTask<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest model)
        {
            return await AnalyzeAsync(model, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<MessageResponse> DeleteAsync(long id)
        {
            return await DeleteAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest model)
        {
            return await FinishAsync(uuid, model, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new LaunchFinishedResponse());
        }

        public async ValueTask<LaunchResponse> GetAsync(long id)
        {
            return await GetAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<LaunchResponse> GetAsync(string uuid)
        {
            return await GetAsync(uuid, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<Content<LaunchResponse>> GetAsync(FilterOption filterOption)
        {
            return await GetAsync(filterOption, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<LaunchResponse> GetAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask<LaunchResponse> GetAsync(string uuid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Content<LaunchResponse>> GetAsync()
        {
            return await GetAsync(CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<Content<LaunchResponse>> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync(filterOption: null, cancellationToken).ConfigureAwait(false);
        }

        public ValueTask<Content<LaunchResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption)
        {
            return await GetDebugAsync(filterOption, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<Content<LaunchResponse>> GetDebugAsync()
        {
            return await GetDebugAsync(filterOption: null, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<Content<LaunchResponse>> GetDebugAsync(CancellationToken cancellationToken)
        {
            return await GetDebugAsync(filterOption: null, cancellationToken).ConfigureAwait(false);
        }

        public ValueTask<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<LaunchResponse> MergeAsync(MergeLaunchesRequest model)
        {
            return await MergeAsync(model, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<LaunchResponse> MergeAsync(MergeLaunchesRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<LaunchCreatedResponse> StartAsync(StartLaunchRequest request)
        {
            return await StartAsync(request, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<LaunchCreatedResponse> StartAsync(StartLaunchRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new LaunchCreatedResponse { Uuid = Guid.NewGuid().ToString() });
        }

        public async ValueTask<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest model)
        {
            return await StopAsync(id, model, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest model)
        {
            return await UpdateAsync(id, model, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class NopTestItemResource : ITestItemResource
    {
        public async ValueTask<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest model)
        {
            return await AssignIssuesAsync(model, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<MessageResponse> DeleteAsync(long id)
        {
            return await DeleteAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<MessageResponse> FinishAsync(string id, FinishTestItemRequest model)
        {
            return await FinishAsync(id, model, CancellationToken.None);
        }

        public async ValueTask<MessageResponse> FinishAsync(string uuid, FinishTestItemRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new MessageResponse());
        }

        public async ValueTask<TestItemResponse> GetAsync(long id)
        {
            return await GetAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<TestItemResponse> GetAsync(string uuid)
        {
            return await GetAsync(uuid, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<Content<TestItemResponse>> GetAsync(FilterOption filterOption)
        {
            return await GetAsync(filterOption, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<TestItemResponse> GetAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TestItemResponse> GetAsync(string uuid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Content<TestItemResponse>> GetAsync()
        {
            return GetAsync(filterOption: null, CancellationToken.None);
        }

        public ValueTask<Content<TestItemResponse>> GetAsync(CancellationToken cancellationToken)
        {
            return GetAsync(filterOption: null, cancellationToken);
        }

        public ValueTask<Content<TestItemResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Content<TestItemHistoryContainer>> GetHistoryAsync(long testItemId, int depth)
        {
            return await GetHistoryAsync(testItemId, depth, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<Content<TestItemHistoryContainer>> GetHistoryAsync(long id, int depth, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<TestItemCreatedResponse> StartAsync(StartTestItemRequest model)
        {
            return await StartAsync(model, CancellationToken.None);
        }

        public async ValueTask<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model)
        {
            return await StartAsync(uuid, model, CancellationToken.None);
        }

        public async ValueTask<TestItemCreatedResponse> StartAsync(StartTestItemRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new TestItemCreatedResponse { Uuid = Guid.NewGuid().ToString() });
        }

        public async ValueTask<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new TestItemCreatedResponse { Uuid = Guid.NewGuid().ToString() });
        }

        public async ValueTask<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest model)
        {
            return await UpdateAsync(id, model, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class NopLogItemResourse : ILogItemResource
    {
        public async ValueTask<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest model)
        {
            return await CreateAsync(model, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<LogItemsCreatedResponse> CreateAsync(CreateLogItemRequest[] models)
        {
            return await CreateAsync(models, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new LogItemCreatedResponse());
        }

        public async ValueTask<LogItemsCreatedResponse> CreateAsync(CreateLogItemRequest[] requests, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new LogItemsCreatedResponse());
        }

        public async ValueTask<MessageResponse> DeleteAsync(long id)
        {
            return await DeleteAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<LogItemResponse> GetAsync(long id)
        {
            return await GetAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<LogItemResponse> GetAsync(string uuid)
        {
            return await GetAsync(uuid, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<Content<LogItemResponse>> GetAsync(FilterOption filterOption)
        {
            return await GetAsync(filterOption, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<LogItemResponse> GetAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask<LogItemResponse> GetAsync(string uuid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Content<LogItemResponse>> GetAsync()
        {
            return await GetAsync(filterOption: null, CancellationToken.None).ConfigureAwait(false);
        }

        public async ValueTask<Content<LogItemResponse>> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync(filterOption: null, cancellationToken).ConfigureAwait(false);
        }

        public ValueTask<Content<LogItemResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<byte[]> GetBinaryDataAsync(string id)
        {
            return await GetBinaryDataAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        public ValueTask<byte[]> GetBinaryDataAsync(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
