using Moq;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Internal.Delegating;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Tests.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests.Reporter
{
    public class LogsReporterFixture
    {
        Mock<ITestReporter> _testReporter;
        IRequestExecuter _requestExecuter;
        IExtensionManager _extensionManager;
        Mock<ILogRequestAmender> _logRequestAmender;

        public LogsReporterFixture()
        {
            _testReporter = new Mock<ITestReporter>();
            _testReporter.SetupGet(r => r.StartTask).Returns(() => Task.FromResult(0));
            _testReporter.SetupGet(r => r.Info).Returns(() => new TestInfo { });

            _requestExecuter = new NoneRetryRequestExecuter(null);

            _extensionManager = new ExtensionManager();

            _logRequestAmender = new Mock<ILogRequestAmender>();
        }

        [Fact]
        public void ShouldSendRequestPerLogItem()
        {
            var service = new MockServiceBuilder().Build();

            var logsReporter = new LogsReporter(_testReporter.Object, service.Object, _extensionManager, _requestExecuter, _logRequestAmender.Object);
            logsReporter.BatchCapacity = 1;

            for (int i = 0; i < 50; i++)
            {
                logsReporter.Log(new CreateLogItemRequest
                {
                    Text = "a",
                    Time = DateTime.UtcNow
                });
            }

            logsReporter.Sync();

            service.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>()), Times.Exactly(50));
        }

        [Fact]
        public void ShouldSendBatchedRequests()
        {
            var service = new MockServiceBuilder().Build();

            var logsReporter = new LogsReporter(_testReporter.Object, service.Object, _extensionManager, _requestExecuter, _logRequestAmender.Object);

            for (int i = 0; i < 50; i++)
            {
                logsReporter.Log(new CreateLogItemRequest
                {
                    Text = "a",
                    Time = DateTime.UtcNow
                });
            }

            logsReporter.Sync();

            service.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>()), Times.Exactly(5));
        }

        [Fact]
        public void ShouldSendRequestsEvenIfPreviousFailed()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>())).Throws<Exception>();

            var logsReporter = new LogsReporter(_testReporter.Object, service.Object, _extensionManager, _requestExecuter, _logRequestAmender.Object);
            logsReporter.BatchCapacity = 1;

            for (int i = 0; i < 2; i++)
            {
                logsReporter.Log(new CreateLogItemRequest
                {
                    Text = "a",
                    Time = DateTime.UtcNow
                });
            }

            try
            {
                logsReporter.Sync();
            }
            catch (Exception) { }

            service.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>()), Times.Exactly(2));
        }

        [Fact]
        public void ShouldSendAsSeparateRequestPerLogWithAttachment()
        {
            var service = new MockServiceBuilder().Build();

            var logsReporter = new LogsReporter(_testReporter.Object, service.Object, _extensionManager, _requestExecuter, _logRequestAmender.Object);

            for (int i = 0; i < 2; i++)
            {
                logsReporter.Log(new CreateLogItemRequest
                {
                    Text = "a",
                    Time = DateTime.UtcNow,
                    Attach = new LogItemAttach()
                });
            }

            logsReporter.Sync();

            service.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>()), Times.Exactly(2));
        }

        [Fact]
        public void ShouldSendAsSeparateRequestPerLogWithAttachmentIncludingWithoutAttachment()
        {
            var service = new MockServiceBuilder().Build();

            var logsReporter = new LogsReporter(_testReporter.Object, service.Object, _extensionManager, _requestExecuter, _logRequestAmender.Object);

            var withoutAttachment = new CreateLogItemRequest
            {
                Text = "a",
                Time = DateTime.UtcNow
            };
            var withAttachment = new CreateLogItemRequest
            {
                Text = "a",
                Time = DateTime.UtcNow,
                Attach = new LogItemAttach()
            };

            logsReporter.Log(withAttachment);
            logsReporter.Log(withoutAttachment);
            logsReporter.Log(withAttachment);
            logsReporter.Log(withoutAttachment);

            logsReporter.Sync();

            service.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>()), Times.Exactly(2));
        }

    }
}
