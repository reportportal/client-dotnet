using Moq;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Tests.Helpers;
using System.Collections.Generic;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.LogFormatter
{
    public class LogFormatterTest
    {
        [Fact]
        public void ShouldInvokeFormatLogMethod()
        {
            var service = new MockServiceBuilder().Build();
            var logFormatter = new Mock<ILogFormatter>();
            var extensionManager = new Mock<IExtensionManager>();
            extensionManager.Setup(p => p.LogFormatters).Returns(new List<ILogFormatter> { logFormatter.Object });

            var launchScheduler = new LaunchReporterBuilder(service.Object).With(extensionManager.Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            launchReporter.Sync();

            logFormatter.Verify(lf => lf.FormatLog(It.IsAny<CreateLogItemRequest>()), Times.Once);
        }
    }
}
