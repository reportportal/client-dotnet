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
    public class BigTreeInvocation
    {
        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(1, 1, 1)]
        [InlineData(5, 10000, 0)]
        [InlineData(5, 10000, 10)]
        public void SuccessReporting(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            launchReporter.Sync();

            service.Verify(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.StartTestItemAsync(null, It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch));
            service.Verify(s => s.FinishTestItemAsync(null, It.IsAny<Client.Requests.FinishTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch + suitesPerLaunch));
            service.Verify(s => s.AddLogItemAsync(It.IsAny<Client.Requests.AddLogItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite * logsPerTest));

            Assert.Equal(nameof(Client.Service.StartLaunchAsync), service.Invocations.First().Method.Name);
            Assert.Equal(nameof(Client.Service.FinishLaunchAsync), service.Invocations.Last().Method.Name);
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
            service.Verify(s => s.StartTestItemAsync(null, It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch));
            service.Verify(s => s.AddLogItemAsync(It.IsAny<Client.Requests.AddLogItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite * logsPerTest));
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 1, 2)]
        [InlineData(5, 5, 0)]
        public void FailedFinishTestItemShouldRaiseExceptionAtFinishLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.FinishTestItemAsync(null, It.IsAny<Client.Requests.FinishTestItemRequest>())).Throws(new Exception());

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.StartTestItemAsync(null, It.IsAny<Client.Requests.StartTestItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite));
            service.Verify(s => s.FinishTestItemAsync(null, It.IsAny<Client.Requests.FinishTestItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite));
        }

        [Theory]
        [InlineData(100, 1, 1)]
        [InlineData(1, 100, 10000)]
        [InlineData(100, 100, 1)]
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
        public void StartSuiteTimeout()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>())).Throws<TaskCanceledException>();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);
        }

        [Fact]
        public void StartTestItemTimeout()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.StartTestItemAsync(null, It.IsAny<Client.Requests.StartTestItemRequest>())).Throws<TaskCanceledException>();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);
        }

        [Fact]
        public void FinishTestItemTimeout()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.FinishTestItemAsync(null, It.IsAny<Client.Requests.FinishTestItemRequest>())).Throws<TaskCanceledException>();

            var launchScheduler = new LaunchScheduler(service.Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.FinishLaunchAsync(null, It.IsAny<Client.Requests.FinishLaunchRequest>(), false), Times.Never);
        }
    }
}
