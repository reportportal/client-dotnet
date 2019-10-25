using FluentAssertions;
using Moq;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests.Faked
{
    public class ReporterTest
    {
        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(1, 1, 1)]
        [InlineData(5, 100, 0)]
        [InlineData(5, 100, 10)]
        public void SuccessReporting(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            launchReporter.Sync();

            service.Verify(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.StartTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch));
            service.Verify(s => s.FinishTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch + suitesPerLaunch));
            service.Verify(s => s.AddLogItemAsync(It.IsAny<Client.Requests.AddLogItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite * logsPerTest));

            Assert.Equal(nameof(Client.Service.StartLaunchAsync), service.Invocations.First().Method.Name);
            Assert.Equal(nameof(Client.Service.FinishLaunchAsync), service.Invocations.Last().Method.Name);

            launchReporter.ChildTestReporters.Select(s => s.TestInfo.Id).Should().OnlyHaveUniqueItems();
            launchReporter.ChildTestReporters.SelectMany(s => s.ChildTestReporters).Select(t => t.TestInfo.Id).Should().OnlyHaveUniqueItems();
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(10, 10, 10)]
        public void FailedLogsShouldNotAffectFinishingLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.AddLogItemAsync(It.IsAny<Client.Requests.AddLogItemRequest>())).Throws<Exception>();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            launchReporter.Sync();

            service.Verify(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.StartTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch));
            service.Verify(s => s.AddLogItemAsync(It.IsAny<Client.Requests.AddLogItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite * logsPerTest));
            service.Verify(s => s.FinishTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch + suitesPerLaunch));
            service.Verify(s => s.FinishLaunchAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishLaunchRequest>(), It.IsAny<bool>()), Times.Once);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(10, 10, 10)]
        public void CanceledLogsShouldNotAffectFinishingLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.AddLogItemAsync(It.IsAny<Client.Requests.AddLogItemRequest>())).Throws<TaskCanceledException>();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            launchReporter.Sync();

            service.Verify(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.StartTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch));
            service.Verify(s => s.AddLogItemAsync(It.IsAny<Client.Requests.AddLogItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite * logsPerTest * 3));
            service.Verify(s => s.FinishTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch + suitesPerLaunch));
            service.Verify(s => s.FinishLaunchAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishLaunchRequest>(), It.IsAny<bool>()), Times.Once);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 1, 2)]
        [InlineData(5, 5, 0)]
        public void FailedFinishTestItemShouldRaiseExceptionAtFinishLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.FinishTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishTestItemRequest>())).Throws(new Exception());

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.StartTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite));
            service.Verify(s => s.FinishTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishTestItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite));
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(10, 10, 2)]
        [InlineData(50, 50, 0)]
        public void CanceledFinishTestItemShouldRaiseExceptionAtFinishLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.FinishTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishTestItemRequest>())).Throws<TaskCanceledException>();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.StartTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite));
            service.Verify(s => s.FinishTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishTestItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite * 3));
            service.Verify(s => s.FinishLaunchAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishLaunchRequest>(), false), Times.Never);
        }

        [Theory]
        [InlineData(100, 1, 1)]
        [InlineData(1, 100, 100)]
        [InlineData(100, 10, 1)]
        public void FailedStartSuiteItemShouldRaiseExceptionAtFinishLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>())).Throws<Exception>();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.StartTestItemAsync(null, It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Never);
            service.Verify(s => s.FinishTestItemAsync(null, It.IsAny<Client.Requests.FinishTestItemRequest>()), Times.Never);
        }

        [Theory]
        [InlineData(100, 1, 1)]
        [InlineData(1, 10, 100)]
        [InlineData(10, 10, 1)]
        public void CanceledStartSuiteItemShouldRaiseExceptionAtFinishLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>())).Throws<TaskCanceledException>();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch * 3));
            service.Verify(s => s.StartTestItemAsync(null, It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Never);
            service.Verify(s => s.FinishTestItemAsync(null, It.IsAny<Client.Requests.FinishTestItemRequest>()), Times.Never);
            service.Verify(s => s.FinishLaunchAsync(null, It.IsAny<Client.Requests.FinishLaunchRequest>(), false), Times.Never);
        }

        [Fact]
        public void StartLaunchScheduling()
        {
            var service = new MockServiceBuilder().Build();

            var launchReporters = new List<Mock<LaunchReporter>>();

            for (int i = 0; i < 1000; i++)
            {
                var launchReporter = new Mock<LaunchReporter>(service.Object);

                launchReporter.Object.Start(new Client.Requests.StartLaunchRequest
                {
                    Name = $"ReportPortal Shared {i}",
                    StartTime = DateTime.UtcNow
                });

                launchReporters.Add(launchReporter);
            }

            for (int i = 0; i < 1000; i++)
            {
                var launchReporter = launchReporters[i];

                Assert.NotNull(launchReporter.Object.StartTask);

                launchReporter.Object.Sync();

                Assert.Equal($"ReportPortal Shared {i}", launchReporter.Object.LaunchInfo.Name);
            }

            service.Verify(s => s.StartLaunchAsync(It.IsAny<Client.Requests.StartLaunchRequest>()), Times.Exactly(1000));
        }

        [Fact]
        public void StartLaunchTimeout()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.StartLaunchAsync(It.IsAny<Client.Requests.StartLaunchRequest>())).Throws<TaskCanceledException>();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
        }

        [Fact]
        public void StartTestItemTimeout()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.StartTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.StartTestItemRequest>())).Throws<TaskCanceledException>();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);
        }

        [Fact]
        public void FinishTestItemTimeout()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.FinishTestItemAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishTestItemRequest>())).Throws<TaskCanceledException>();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.FinishLaunchAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishLaunchRequest>(), false), Times.Never);
        }

        [Fact]
        public void LogsReportingShouldBeOneByOne()
        {
            var logDelay = TimeSpan.FromMilliseconds(100);

            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.AddLogItemAsync(It.IsAny<Client.Requests.AddLogItemRequest>())).Returns(async () => { await Task.Delay(logDelay); return new Client.Models.LogItem(); });

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(1, 30, 30);

            launchReporter.ExecutionTimeOf(l => l.Sync()).Should().BeGreaterOrEqualTo(TimeSpan.FromTicks(logDelay.Ticks * 10));

            launchReporter.Sync();

            service.Verify(s => s.FinishLaunchAsync(It.IsAny<string>(), It.IsAny<Client.Requests.FinishLaunchRequest>(), false), Times.Once);
            service.Verify(s => s.AddLogItemAsync(It.IsAny<Client.Requests.AddLogItemRequest>()), Times.Exactly(30 * 30));
        }
    }
}
