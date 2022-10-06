using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Internal.Delegating;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Reporter.Statistics;
using ReportPortal.Shared.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests.Reporter
{
    public class LogsReporterFixture
    {
        readonly Mock<ITestReporter> _testReporter;
        readonly IConfiguration _configuration;
        readonly IRequestExecuter _requestExecuter;
        readonly IExtensionManager _extensionManager;
        readonly Mock<ILogRequestAmender> _logRequestAmender;
        readonly ReportEventsSource _reportEventsSource;

        public LogsReporterFixture()
        {
            _testReporter = new Mock<ITestReporter>();
            _testReporter.SetupGet(r => r.StatisticsCounter).Returns(() => new LaunchStatisticsCounter());
            _testReporter.SetupGet(r => r.StartTask).Returns(() => Task.FromResult(0));
            _testReporter.SetupGet(r => r.Info).Returns(() => new TestInfo { });

            _configuration = new ConfigurationBuilder().Build();
            _requestExecuter = new NoneRetryRequestExecuter(null);
            _reportEventsSource = new ReportEventsSource();

            _extensionManager = new ExtensionManager();

            _logRequestAmender = new Mock<ILogRequestAmender>();
        }

        [Fact]
        public void ShouldSendRequestPerLogItem()
        {
            var service = new MockServiceBuilder().Build();

            var logsReporter = new LogsReporter(_testReporter.Object, service.Object, _configuration, _extensionManager, _requestExecuter, _logRequestAmender.Object, _reportEventsSource)
            {
                BatchCapacity = 1
            };

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

            var logsReporter = new LogsReporter(_testReporter.Object, service.Object, _configuration, _extensionManager, _requestExecuter, _logRequestAmender.Object, _reportEventsSource);

            for (int i = 0; i < 50; i++)
            {
                logsReporter.Log(new CreateLogItemRequest
                {
                    Text = "a",
                    Time = DateTime.UtcNow
                });
            }

            logsReporter.Sync();

            // sometimes on slow machines it's not exact 5 invocations
            service.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>()), Times.Between(5, 6, Moq.Range.Inclusive));
        }

        [Fact]
        public void ShouldSendRequestsEvenIfPreviousFailed()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>())).Throws<Exception>();

            var logsReporter = new LogsReporter(_testReporter.Object, service.Object, _configuration, _extensionManager, _requestExecuter, _logRequestAmender.Object, _reportEventsSource)
            {
                BatchCapacity = 1
            };

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

            var logsReporter = new LogsReporter(_testReporter.Object, service.Object, _configuration, _extensionManager, _requestExecuter, _logRequestAmender.Object, _reportEventsSource);

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

            var logsReporter = new LogsReporter(_testReporter.Object, service.Object, _configuration, _extensionManager, _requestExecuter, _logRequestAmender.Object, _reportEventsSource);

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

        [Fact]
        public void ShouldBeThreadSafeWhenSchedulingLogRequests()
        {
            var logItemRequestTexts = new List<string>();

            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>()))
                .Callback<CreateLogItemRequest[]>(rqs => { foreach (var rq in rqs) logItemRequestTexts.Add(rq.Text); })
                .Returns(() => Task.FromResult(new Client.Abstractions.Responses.LogItemsCreatedResponse()));

            var logsReporter = new LogsReporter(_testReporter.Object, service.Object, _configuration, _extensionManager, _requestExecuter, _logRequestAmender.Object, _reportEventsSource);

            Parallel.For(0, 1000, (i) => logsReporter.Log(new CreateLogItemRequest
            {
                Text = i.ToString(),
                Time = DateTime.UtcNow
            }));

            logsReporter.Sync();

            // we have scheduled 1000 log items which will be consumed by 10 items in loop in background (happy path)
            // sometimes consumer iterates faster than producer
            service.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>()), Times.Between(100, 200, Moq.Range.Inclusive));

            logItemRequestTexts.Should().HaveCount(1000);
            logItemRequestTexts.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void ShouldAmendLogDatetimeForLaunch()
        {
            var startTime = DateTime.UtcNow;

            var launchReporter = new Mock<ILaunchReporter>();
            launchReporter.Setup(l => l.Info.StartTime).Returns(startTime);

            var amender = new LaunchLogRequestAmender(launchReporter.Object);

            var logRequest = new CreateLogItemRequest() { Time = startTime.AddMinutes(-1) };

            amender.Amend(logRequest);

            logRequest.Time.Should().Be(startTime);
        }

        [Fact]
        public void ShouldAmendLogDatetimeForTestItem()
        {
            var startTime = DateTime.UtcNow;

            var testReporter = new Mock<ITestReporter>();
            testReporter.Setup(l => l.Info.StartTime).Returns(startTime);

            var amender = new TestLogRequestAmender(testReporter.Object);

            var logRequest = new CreateLogItemRequest() { Time = startTime.AddMinutes(-1) };

            amender.Amend(logRequest);

            logRequest.Time.Should().Be(startTime);
        }
    }
}
