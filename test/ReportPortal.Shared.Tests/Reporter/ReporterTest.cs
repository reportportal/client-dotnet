using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Tests.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests.Reporter
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

            var launchScheduler = new LaunchReporterBuilder(service.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            launchReporter.Sync();

            service.Verify(s => s.TestItem.StartAsync(It.IsAny<StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.TestItem.StartAsync(It.IsAny<string>(), It.IsAny<StartTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch));
            service.Verify(s => s.TestItem.FinishAsync(It.IsAny<string>(), It.IsAny<FinishTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch + suitesPerLaunch));

            launchReporter.ChildTestReporters.Select(s => s.Info.Uuid).Should().OnlyHaveUniqueItems();
            launchReporter.ChildTestReporters.SelectMany(s => s.ChildTestReporters).Select(t => t.Info.Uuid).Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void MixingTestsAndLogs()
        {
            var service = new MockServiceBuilder().Build();

            var launchReporter = new LaunchReporter(service.Object, null, null, new ExtensionManager());
            launchReporter.Start(new StartLaunchRequest { StartTime = DateTime.UtcNow });

            for (int i = 0; i < 10; i++)
            {
                var suite = launchReporter.StartChildTestReporter(new StartTestItemRequest { StartTime = DateTime.UtcNow });

                suite.Log(new CreateLogItemRequest { Time = DateTime.UtcNow });

                for (int j = 0; j < 20; j++)
                {
                    var test = suite.StartChildTestReporter(new StartTestItemRequest { StartTime = DateTime.UtcNow });

                    test.Log(new CreateLogItemRequest { Time = DateTime.UtcNow });

                    test.Finish(new FinishTestItemRequest { EndTime = DateTime.UtcNow });
                }

                suite.Log(new CreateLogItemRequest { Time = DateTime.UtcNow });

                suite.Finish(new FinishTestItemRequest { EndTime = DateTime.UtcNow });
            }

            launchReporter.Finish(new FinishLaunchRequest { EndTime = DateTime.UtcNow });

            launchReporter.Sync();
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(10, 10, 10)]
        public void FailedLogsShouldNotAffectFinishingLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>())).Throws<Exception>();

            var requestExecuterFactory = new MockRequestExecuterFactoryBuilder().Build();

            var launchScheduler = new LaunchReporterBuilder(service.Object).With(requestExecuterFactory.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            launchReporter.Sync();

            service.Verify(s => s.Launch.StartAsync(It.IsAny<StartLaunchRequest>()), Times.Exactly(1));
            service.Verify(s => s.TestItem.StartAsync(It.IsAny<StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.TestItem.StartAsync(It.IsAny<string>(), It.IsAny<StartTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch));
            service.Verify(s => s.TestItem.FinishAsync(It.IsAny<string>(), It.IsAny<FinishTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch + suitesPerLaunch));
            service.Verify(s => s.Launch.FinishAsync(It.IsAny<string>(), It.IsAny<FinishLaunchRequest>()), Times.Once);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(10, 10, 10)]
        public void CanceledLogsShouldNotAffectFinishingLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>())).Throws<TaskCanceledException>();
            service.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>())).Throws<TaskCanceledException>();

            var requestExecuterFactory = new MockRequestExecuterFactoryBuilder().Build();

            var launchScheduler = new LaunchReporterBuilder(service.Object).With(requestExecuterFactory.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            launchReporter.Sync();

            service.Verify(s => s.TestItem.StartAsync(It.IsAny<StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.TestItem.StartAsync(It.IsAny<string>(), It.IsAny<StartTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch));
            service.Verify(s => s.TestItem.FinishAsync(It.IsAny<string>(), It.IsAny<FinishTestItemRequest>()), Times.Exactly(testsPerSuite * suitesPerLaunch + suitesPerLaunch));
            service.Verify(s => s.Launch.FinishAsync(It.IsAny<string>(), It.IsAny<FinishLaunchRequest>()), Times.Once);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 1, 2)]
        [InlineData(5, 5, 0)]
        public void FailedFinishTestItemShouldRaiseExceptionAtFinishLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.TestItem.FinishAsync(It.IsAny<string>(), It.IsAny<FinishTestItemRequest>())).Throws(new Exception());

            var requestExecuterFactory = new MockRequestExecuterFactoryBuilder().Build();

            var launchScheduler = new LaunchReporterBuilder(service.Object).With(requestExecuterFactory.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.TestItem.StartAsync(It.IsAny<StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.TestItem.StartAsync(It.IsAny<string>(), It.IsAny<StartTestItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite));
            service.Verify(s => s.TestItem.FinishAsync(It.IsAny<string>(), It.IsAny<FinishTestItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite));
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(10, 10, 2)]
        [InlineData(50, 50, 0)]
        public void CanceledFinishTestItemShouldRaiseExceptionAtFinishLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.TestItem.FinishAsync(It.IsAny<string>(), It.IsAny<FinishTestItemRequest>())).Throws<TaskCanceledException>();

            var requestExecuterFactory = new MockRequestExecuterFactoryBuilder().Build();

            var launchScheduler = new LaunchReporterBuilder(service.Object).With(requestExecuterFactory.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.TestItem.StartAsync(It.IsAny<StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.TestItem.StartAsync(It.IsAny<string>(), It.IsAny<StartTestItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite));
            service.Verify(s => s.TestItem.FinishAsync(It.IsAny<string>(), It.IsAny<FinishTestItemRequest>()), Times.Exactly(suitesPerLaunch * testsPerSuite));
            service.Verify(s => s.Launch.FinishAsync(It.IsAny<string>(), It.IsAny<FinishLaunchRequest>()), Times.Never);
        }

        [Theory]
        [InlineData(100, 1, 1)]
        [InlineData(1, 100, 100)]
        [InlineData(100, 10, 1)]
        public void FailedStartSuiteItemShouldRaiseExceptionAtFinishLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.TestItem.StartAsync(It.IsAny<StartTestItemRequest>())).Throws<Exception>();

            var requestExecuterFactory = new MockRequestExecuterFactoryBuilder().Build();

            var launchScheduler = new LaunchReporterBuilder(service.Object).With(requestExecuterFactory.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.TestItem.StartAsync(It.IsAny<StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.TestItem.StartAsync(null, It.IsAny<StartTestItemRequest>()), Times.Never);
            service.Verify(s => s.TestItem.FinishAsync(null, It.IsAny<FinishTestItemRequest>()), Times.Never);
        }

        [Theory]
        [InlineData(100, 1, 1)]
        [InlineData(1, 10, 100)]
        [InlineData(10, 10, 1)]
        public void CanceledStartSuiteItemShouldRaiseExceptionAtFinishLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.TestItem.StartAsync(It.IsAny<StartTestItemRequest>())).Throws<TaskCanceledException>();

            var requestExecuterFactory = new MockRequestExecuterFactoryBuilder().Build();

            var launchScheduler = new LaunchReporterBuilder(service.Object).With(requestExecuterFactory.Object);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.TestItem.StartAsync(It.IsAny<StartTestItemRequest>()), Times.Exactly(suitesPerLaunch));
            service.Verify(s => s.TestItem.StartAsync(null, It.IsAny<StartTestItemRequest>()), Times.Never);
            service.Verify(s => s.TestItem.FinishAsync(null, It.IsAny<FinishTestItemRequest>()), Times.Never);
            service.Verify(s => s.Launch.FinishAsync(null, It.IsAny<FinishLaunchRequest>()), Times.Never);
        }

        [Fact]
        public void StartLaunchScheduling()
        {
            var service = new MockServiceBuilder().Build();

            var launchReporters = new List<Mock<LaunchReporter>>();

            for (int i = 0; i < 100; i++)
            {
                var launchReporter = new Mock<LaunchReporter>(service.Object, null, null, new ExtensionManager());

                launchReporter.Object.Start(new StartLaunchRequest
                {
                    Name = $"ReportPortal Shared {i}",
                    StartTime = DateTime.UtcNow
                });

                launchReporters.Add(launchReporter);
            }

            for (int i = 0; i < 100; i++)
            {
                var launchReporter = launchReporters[i];

                Assert.NotNull(launchReporter.Object.StartTask);

                launchReporter.Object.Sync();

                Assert.Equal($"ReportPortal Shared {i}", launchReporter.Object.Info.Name);
            }

            service.Verify(s => s.Launch.StartAsync(It.IsAny<StartLaunchRequest>()), Times.Exactly(100));
        }

        [Fact]
        public void StartLaunchTimeout()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.Launch.StartAsync(It.IsAny<StartLaunchRequest>())).Throws<TaskCanceledException>();

            var requestExecuterFactory = new MockRequestExecuterFactoryBuilder().Build();

            var launchScheduler = new LaunchReporterBuilder(service.Object).With(requestExecuterFactory.Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
        }

        [Fact]
        public void StartTestItemTimeout()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.TestItem.StartAsync(It.IsAny<string>(), It.IsAny<StartTestItemRequest>())).Throws<TaskCanceledException>();

            var launchScheduler = new LaunchReporterBuilder(service.Object).With(new MockRequestExecuterFactoryBuilder().Build().Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);
        }

        [Fact]
        public void FinishTestItemTimeout()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.TestItem.FinishAsync(It.IsAny<string>(), It.IsAny<FinishTestItemRequest>())).Throws<TaskCanceledException>();

            var requestExecuterFactory = new MockRequestExecuterFactoryBuilder().Build();

            var launchScheduler = new LaunchReporterBuilder(service.Object).With(requestExecuterFactory.Object);
            var launchReporter = launchScheduler.Build(1, 1, 1);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.Sync());
            Assert.Contains("Cannot finish launch", exp.Message);

            service.Verify(s => s.Launch.FinishAsync(It.IsAny<string>(), It.IsAny<FinishLaunchRequest>()), Times.Never);
        }

        [Fact]
        public void LogsReportingShouldBeOneByOne()
        {
            var requests = new ConcurrentBag<CreateLogItemRequest[]>();

            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>())).ReturnsAsync(new LogItemsCreatedResponse())
                .Callback<CreateLogItemRequest[]>((arg) => requests.Add(arg));

            var launchScheduler = new LaunchReporterBuilder(service.Object);
            var launchReporter = launchScheduler.Build(1, 30, 30);

            launchReporter.Sync();

            service.Verify(s => s.Launch.FinishAsync(It.IsAny<string>(), It.IsAny<FinishLaunchRequest>()), Times.Once);
            service.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>()), Times.AtLeast(30 * 30 / 10)); // logs buffer size

            foreach (var bufferedRequests in requests)
            {
                bufferedRequests.Select(r => r.Text).Should().BeInAscendingOrder();
            }
        }

        [Fact]
        public void FinishLaunchWhenChildTestItemIsNotScheduledToFinish()
        {
            var service = new MockServiceBuilder().Build();

            var launch = new LaunchReporter(service.Object, null, null, new ExtensionManager());
            launch.Start(new StartLaunchRequest { });

            var test = launch.StartChildTestReporter(new StartTestItemRequest { });

            var exp = Assert.Throws<InsufficientExecutionStackException>(() => launch.Finish(new FinishLaunchRequest { }));
            Assert.Contains("are not scheduled to finish yet", exp.Message);
        }

        [Fact]
        public void FinishLaunchWhichIsAlreadyFinished()
        {
            var service = new MockServiceBuilder().Build();

            var launch = new LaunchReporter(service.Object, null, null, new Mock<IExtensionManager>().Object);
            launch.Start(new StartLaunchRequest { });
            launch.Finish(new FinishLaunchRequest());
            launch.Invoking(l => l.Finish(new FinishLaunchRequest())).Should().Throw<InsufficientExecutionStackException>().And.Message.Should().Contain("already scheduled for finishing");
        }

        [Fact]
        public void FinishLaunchWhichIsNotStarted()
        {
            var service = new MockServiceBuilder().Build();

            var launch = new LaunchReporter(service.Object, null, null, new ExtensionManager());
            launch.Invoking(l => l.Finish(new FinishLaunchRequest())).Should().Throw<InsufficientExecutionStackException>().And.Message.Should().Contain("wasn't scheduled for starting");
        }

        [Fact]
        public void StartingLaunchWhichIsAlreadyStarted()
        {
            var service = new MockServiceBuilder().Build();

            var launch = new LaunchReporter(service.Object, null, null, new ExtensionManager());
            launch.Start(new StartLaunchRequest { });
            launch.Invoking(l => l.Start(new StartLaunchRequest { })).Should().Throw<InsufficientExecutionStackException>().And.Message.Should().Contain("already scheduled for starting");
        }

        [Fact]
        public void FinishTestItemWhenChildTestItemIsNotScheduledToFinish()
        {
            var service = new MockServiceBuilder().Build();

            var launch = new LaunchReporter(service.Object, null, null, new Mock<IExtensionManager>().Object);
            launch.Start(new StartLaunchRequest { });
            var test = launch.StartChildTestReporter(new StartTestItemRequest());
            var innerTest = test.StartChildTestReporter(new StartTestItemRequest());

            var exp = Assert.Throws<InsufficientExecutionStackException>(() => test.Finish(new FinishTestItemRequest()));
            Assert.Contains("are not scheduled to finish yet", exp.Message);
        }

        [Fact]
        public void ShouldThrowExceptionWhenStartingAlreadyStartedTestItem()
        {
            var service = new MockServiceBuilder().Build();

            var launch = new LaunchReporter(service.Object, null, null, new Mock<IExtensionManager>().Object);
            launch.Start(new StartLaunchRequest());
            var test = launch.StartChildTestReporter(new StartTestItemRequest());
            test.Invoking(t => t.Start(new StartTestItemRequest())).Should().ThrowExactly<InsufficientExecutionStackException>();
        }

        [Fact]
        public void RerunLaunch()
        {
            var service = new MockServiceBuilder().Build();

            var config = new Shared.Configuration.ConfigurationBuilder().Build();
            config.Properties["Launch:RerunOf"] = "any_uuid_of_existing_launch";

            var launch = new LaunchReporter(service.Object, config, null, new Mock<IExtensionManager>().Object);
            launch.Start(new StartLaunchRequest() { StartTime = DateTime.UtcNow });
            launch.Finish(new FinishLaunchRequest() { EndTime = DateTime.UtcNow });
            launch.Sync();

            service.Verify(s => s.Launch.StartAsync(It.IsAny<StartLaunchRequest>()), Times.Once);
            service.Verify(s => s.Launch.FinishAsync(It.IsAny<string>(), It.IsAny<FinishLaunchRequest>()), Times.Once);
        }

        [Fact]
        public void LaunchShouldCareOfFinishTime()
        {
            var launchStartTime = DateTime.UtcNow;

            var service = new MockServiceBuilder().Build();

            var launch = new LaunchReporter(service.Object, null, null, new ExtensionManager());
            launch.Start(new StartLaunchRequest() { StartTime = launchStartTime });
            launch.Finish(new FinishLaunchRequest() { EndTime = launchStartTime.AddDays(-1) });
            launch.Sync();

            launch.Info.FinishTime.Should().Be(launch.Info.StartTime);
        }

        [Fact]
        public void ShouldBeAbleToLogIntoLaunch()
        {
            var service = new MockServiceBuilder().Build();

            var launch = new LaunchReporter(service.Object, null, null, new ExtensionManager());
            launch.Start(new StartLaunchRequest() { StartTime = DateTime.UtcNow });
            launch.Log(new CreateLogItemRequest { Time = DateTime.UtcNow, Text = "log" });
            launch.Finish(new FinishLaunchRequest() { EndTime = DateTime.UtcNow });
            launch.Sync();

            service.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>()), Times.Once);
        }

        [Fact]
        public void CannotLogIfLaunchNotStarted()
        {
            var service = new MockServiceBuilder().Build();

            var launch = new LaunchReporter(service.Object, null, null, new ExtensionManager());
            Action act = () => launch.Log(new CreateLogItemRequest { Time = DateTime.UtcNow, Text = "log" });

            act.Should().Throw<InsufficientExecutionStackException>().WithMessage("*launch wasn't scheduled for starting*");
        }

        [Fact]
        public void ShouldNotLogIfLaunchFailedToStart()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.Launch.StartAsync(It.IsAny<StartLaunchRequest>())).Throws(new Exception());

            var launch = new LaunchReporter(service.Object, null, null, new ExtensionManager());
            launch.Start(new StartLaunchRequest() { StartTime = DateTime.UtcNow });

            launch.Log(new CreateLogItemRequest { Time = DateTime.UtcNow, Text = "log" });

            Action waitAct = () => launch.Sync();
            waitAct.Should().Throw<Exception>();

            service.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>()), Times.Never);
        }

        [Fact]
        public void FailedLaunchLogShouldNotBreakReporting()
        {
            var service = new MockServiceBuilder().Build();
            service.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>())).Throws(new Exception());

            var launch = new LaunchReporter(service.Object, null, null, new ExtensionManager());
            launch.Start(new StartLaunchRequest() { StartTime = DateTime.UtcNow });
            launch.Log(new CreateLogItemRequest { Time = DateTime.UtcNow, Text = "log" });
            launch.Finish(new FinishLaunchRequest() { EndTime = DateTime.UtcNow });

            launch.Sync();
        }
    }
}
