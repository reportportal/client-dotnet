using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System;
using System.Collections.Generic;
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
        public Task<MessageResponse> AnalyzeAsync(AnalyzeLaunchRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponse> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<LaunchFinishedResponse> FinishAsync(string uuid, FinishLaunchRequest model)
        {
            return await Task.FromResult(new LaunchFinishedResponse());
        }

        public Task<LaunchResponse> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<LaunchResponse> GetAsync(string uuid)
        {
            throw new NotImplementedException();
        }

        public Task<Content<LaunchResponse>> GetAsync(FilterOption filterOption = null)
        {
            throw new NotImplementedException();
        }

        public Task<Content<LaunchResponse>> GetDebugAsync(FilterOption filterOption = null)
        {
            throw new NotImplementedException();
        }

        public Task<LaunchResponse> MergeAsync(MergeLaunchesRequest model)
        {
            throw new NotImplementedException();
        }

        public async Task<LaunchCreatedResponse> StartAsync(StartLaunchRequest request)
        {
            return await Task.FromResult(new LaunchCreatedResponse { Uuid = Guid.NewGuid().ToString() });
        }

        public Task<LaunchFinishedResponse> StopAsync(long id, FinishLaunchRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponse> UpdateAsync(long id, UpdateLaunchRequest model)
        {
            throw new NotImplementedException();
        }
    }

    public class NopTestItemResource : ITestItemResource
    {
        public Task<IEnumerable<Issue>> AssignIssuesAsync(AssignTestItemIssuesRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponse> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<MessageResponse> FinishAsync(string id, FinishTestItemRequest model)
        {
            return await Task.FromResult(new MessageResponse());
        }

        public Task<TestItemResponse> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<TestItemResponse> GetAsync(string uuid)
        {
            throw new NotImplementedException();
        }

        public Task<Content<TestItemResponse>> GetAsync(FilterOption filterOption = null)
        {
            throw new NotImplementedException();
        }

        public Task<Content<TestItemHistoryContainer>> GetHistoryAsync(long testItemId, int depth)
        {
            throw new NotImplementedException();
        }

        public async Task<TestItemCreatedResponse> StartAsync(StartTestItemRequest model)
        {
            return await Task.FromResult(new TestItemCreatedResponse { Uuid = Guid.NewGuid().ToString() });
        }

        public async Task<TestItemCreatedResponse> StartAsync(string uuid, StartTestItemRequest model)
        {
            return await Task.FromResult(new TestItemCreatedResponse { Uuid = Guid.NewGuid().ToString() });
        }

        public Task<MessageResponse> UpdateAsync(long id, UpdateTestItemRequest model)
        {
            throw new NotImplementedException();
        }
    }

    public class NopLogItemResourse : ILogItemResource
    {
        public async Task<LogItemCreatedResponse> CreateAsync(CreateLogItemRequest model)
        {
            return await Task.FromResult(new LogItemCreatedResponse());
        }

        public async Task<LogItemsCreatedResponse> CreateAsync(CreateLogItemRequest[] models)
        {
            return await Task.FromResult(new LogItemsCreatedResponse());
        }

        public Task<MessageResponse> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<LogItemResponse> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<LogItemResponse> GetAsync(string uuid)
        {
            throw new NotImplementedException();
        }

        public Task<Content<LogItemResponse>> GetAsync(FilterOption filterOption = null)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetBinaryDataAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
