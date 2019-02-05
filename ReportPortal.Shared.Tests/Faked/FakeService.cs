using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Tests.Faked
{
    public class FakeService : Service
    {
        public FakeService(Uri uri, string project, string password)
            : base(uri, project, password)
        {

        }
        public FakeService(Uri uri, string project, string password, HttpMessageHandler messageHandler)
            : base(uri, project, password, messageHandler)
        {

        }
        public FakeService(Uri uri, string project, string password, IWebProxy proxy)
            : base(uri, project, password, proxy)
        {

        }

        public int RequestsDelay { get; set; } = 50;

        protected object _startTestItemCounterLockObj = new object();
        public int StartTestItemCounter { get; private set; }

        public override async Task<Launch> StartLaunchAsync(StartLaunchRequest model)
        {
            await Task.Delay(RequestsDelay);
            return await Task.FromResult(new Launch { Id = Guid.NewGuid().ToString() });
        }

        public override async Task<Message> FinishLaunchAsync(string id, FinishLaunchRequest model, bool force = false)
        {
            await Task.Delay(RequestsDelay);
            return await Task.FromResult(new Message());
        }

        public override async Task<TestItem> StartTestItemAsync(StartTestItemRequest model)
        {
            lock (_startTestItemCounterLockObj)
            {
                StartTestItemCounter++;
            }
            await Task.Delay(RequestsDelay);
            return await Task.FromResult(new TestItem { Id = Guid.NewGuid().ToString() });
        }

        public override async Task<TestItem> StartTestItemAsync(string id, StartTestItemRequest model)
        {
            lock (_startTestItemCounterLockObj)
            {
                StartTestItemCounter++;
            }
            await Task.Delay(RequestsDelay);
            return await Task.FromResult(new TestItem { Id = Guid.NewGuid().ToString() });
        }

        protected object _finishTestItemCounterLockObj = new object();
        public int FinishTestItemCounter { get; private set; }
        public override async Task<Message> FinishTestItemAsync(string id, FinishTestItemRequest model)
        {
            lock (_finishTestItemCounterLockObj)
            {
                FinishTestItemCounter++;
            }
            await Task.Delay(RequestsDelay);
            return await Task.FromResult(new Message());
        }

        protected object _addLogtemCounterLockObj = new object();
        public int AddLogItemCounter { get; private set; }
        public override async Task<LogItem> AddLogItemAsync(AddLogItemRequest model)
        {
            lock (_addLogtemCounterLockObj)
            {
                AddLogItemCounter++;
            }
            //await Task.Delay(RequestsDelay);
            return await Task.FromResult(new LogItem());
        }
    }
}
