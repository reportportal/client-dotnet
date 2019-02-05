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
    public class FakeServiceWithFailedFirstFinishTestItemMethod : FakeService
    {
        public FakeServiceWithFailedFirstFinishTestItemMethod(Uri uri, string project, string password)
            : base(uri, project, password)
        {

        }
        public FakeServiceWithFailedFirstFinishTestItemMethod(Uri uri, string project, string password, HttpMessageHandler messageHandler)
            : base(uri, project, password, messageHandler)
        {

        }
        public FakeServiceWithFailedFirstFinishTestItemMethod(Uri uri, string project, string password, IWebProxy proxy)
            : base(uri, project, password, proxy)
        {

        }

        private bool _isFirstTimeFinishTestItem = false;
        public override async Task<Message> FinishTestItemAsync(string id, FinishTestItemRequest model)
        {
            lock(_finishTestItemCounterLockObj)
            {
                if (_isFirstTimeFinishTestItem == false)
                {
                    _isFirstTimeFinishTestItem = true;
                    throw new Exception("FinishTestItemAsync first time fails");
                }
            }

            return await base.FinishTestItemAsync(id, model);
        }
    }
}
