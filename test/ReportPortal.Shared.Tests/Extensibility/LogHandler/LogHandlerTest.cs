using ReportPortal.Client.Requests;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.LogHandler
{
    public class LogHandlerTest : ILogHandler
    {
        public int Order => 10;

        [Fact]
        public void ShouldInvokeHandleLogMethod()
        {
            var service = new MockServiceBuilder().Build();

            var launchScheduler = new LaunchReporterBuilder(service.Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            launchReporter.Sync();

            Log.Info("message from test domain");

            Assert.True(Invoked);
        }

        public bool Handle(AddLogItemRequest logRequest)
        {
            Invoked = true;
            return false;
        }

        private static bool Invoked { get; set; }
    }
}
