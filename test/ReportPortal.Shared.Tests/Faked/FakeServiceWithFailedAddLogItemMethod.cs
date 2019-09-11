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
    public class FakeServiceWithFailedAddLogItemMethod : FakeService
    {
        public FakeServiceWithFailedAddLogItemMethod(Uri uri, string project, string password)
            : base(uri, project, password)
        {

        }
        public FakeServiceWithFailedAddLogItemMethod(Uri uri, string project, string password, HttpMessageHandler messageHandler)
            : base(uri, project, password, messageHandler)
        {

        }
        public FakeServiceWithFailedAddLogItemMethod(Uri uri, string project, string password, IWebProxy proxy)
            : base(uri, project, password, proxy)
        {

        }

        public override async Task<LogItem> AddLogItemAsync(AddLogItemRequest model)
        {
            await Task.Delay(0);
            throw new Exception();
        }
    }
}
