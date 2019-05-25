using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Benchmark.Reporter
{
    public class NopService : Service
    {
        public NopService(Uri uri, string project, string password)
            : base(uri, project, password)
        {

        }
        public NopService(Uri uri, string project, string password, HttpMessageHandler messageHandler)
            : base(uri, project, password, messageHandler)
        {

        }
        public NopService(Uri uri, string project, string password, IWebProxy proxy)
            : base(uri, project, password, proxy)
        {

        }

        public override async Task<Launch> StartLaunchAsync(StartLaunchRequest model)
        {
            return await Task.FromResult(new Launch { Id = Guid.NewGuid().ToString() });
        }

        public override async Task<Message> FinishLaunchAsync(string id, FinishLaunchRequest model, bool force = false)
        {
            return await Task.FromResult(new Message());
        }

        public override async Task<TestItem> StartTestItemAsync(StartTestItemRequest model)
        {
            return await Task.FromResult(new TestItem { Id = Guid.NewGuid().ToString() });
        }

        public override async Task<TestItem> StartTestItemAsync(string id, StartTestItemRequest model)
        {
            return await Task.FromResult(new TestItem { Id = Guid.NewGuid().ToString() });
        }

        public override async Task<Message> FinishTestItemAsync(string id, FinishTestItemRequest model)
        {
            return await Task.FromResult(new Message());
        }

        public override async Task<LogItem> AddLogItemAsync(AddLogItemRequest model)
        {
            return await Task.FromResult(new LogItem());
        }
    }
}
