using ReportPortal.Client.Requests;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Tests.Helpers;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.LogFormatter
{
    public class LogFormatterTest : ILogFormatter
    {
        public int Order => 10;

        [Fact]
        public void ShouldInvokeFormatLogMethod()
        {
            var service = new MockServiceBuilder().Build();

            var launchScheduler = new LaunchReporterBuilder(service.Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            launchReporter.Sync();

            Assert.True(Invoked);
        }

        public bool FormatLog(ref AddLogItemRequest logRequest)
        {
            Invoked = true;
            return false;
        }

        private static bool Invoked { get; set; }
    }
}
