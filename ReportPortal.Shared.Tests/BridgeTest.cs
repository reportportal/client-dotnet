using System;
using System.Collections.Generic;
using System.Text;
using ReportPortal.Client.Requests;
using Xunit;

namespace ReportPortal.Shared.Tests
{

    public class BridgeTest
    {
        public class BridgeTestExtension : IBridgeExtension
        {
            public int Order => 0;

            public bool Handled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public void FormatLog(ref AddLogItemRequest logRequest)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void InitBridgeExtension()
        {
            var extensions = Bridge.Extensions;
            Assert.Equal(3, extensions.Count);
        }
    }
}
