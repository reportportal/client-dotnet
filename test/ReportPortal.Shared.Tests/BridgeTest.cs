using System;
using ReportPortal.Client.Requests;
using ReportPortal.Shared.Extensibility;
using Xunit;

namespace ReportPortal.Shared.Tests
{

    public class BridgeTest
    {
        public class BridgeTestExtension : ILogFormatter
        {
            public int Order => 0;

            public bool FormatLog(ref AddLogItemRequest logRequest)
            {
                return true;
            }
        }

        [Fact]
        public void InitBridgeExtension()
        {
            var extensions = Bridge.LogFormatterExtensions;
            Assert.Equal(3, extensions.Count);
        }
    }
}
