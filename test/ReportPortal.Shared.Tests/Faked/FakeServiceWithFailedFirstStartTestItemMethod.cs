using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace ReportPortal.Shared.Tests.Faked
{
    public class FakeServiceWithFailedFirstStartTestItemMethod : FakeService
    {
        public FakeServiceWithFailedFirstStartTestItemMethod(Uri uri, string project, string password)
            : base(uri, project, password)
        {

        }
        public FakeServiceWithFailedFirstStartTestItemMethod(Uri uri, string project, string password, HttpMessageHandler messageHandler)
            : base(uri, project, password, messageHandler)
        {

        }
        public FakeServiceWithFailedFirstStartTestItemMethod(Uri uri, string project, string password, IWebProxy proxy)
            : base(uri, project, password, proxy)
        {

        }

        private bool _isFirstTimeStartTestItem = false;

        public override Task<TestItem> StartTestItemAsync(StartTestItemRequest model)
        {
            lock (_startTestItemCounterLockObj)
            {
                if (_isFirstTimeStartTestItem == false)
                {
                    _isFirstTimeStartTestItem = true;
                    throw new Exception("StartTestItemAsync first time fails");
                }
            }

            return base.StartTestItemAsync(model);
        }

        public override Task<TestItem> StartTestItemAsync(string id, StartTestItemRequest model)
        {
            lock (_startTestItemCounterLockObj)
            {
                if (_isFirstTimeStartTestItem == false)
                {
                    _isFirstTimeStartTestItem = true;
                    throw new Exception("StartTestItemAsync first time fails");
                }
            }

            return base.StartTestItemAsync(id, model);
        }
    }
}
