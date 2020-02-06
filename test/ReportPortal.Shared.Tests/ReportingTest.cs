using ReportPortal.Client;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Tests.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    public class ReportingTest
    {
        private Service _service = new Service(new Uri("https://beta.demo.reportportal.io/api/v1/"), "default_personal", "2908ac62-6855-4f70-bd38-64d5589a4073");

        [Fact]
        public async Task BigAsyncRealTree()
        {
            var launchScheduler = new LaunchReporterBuilder(_service);
            var launchReporter = launchScheduler.Build(10, 3, 1);

            launchReporter.Sync();

            var launch = await _service.Launch.GetAsync(launchReporter.LaunchInfo.Uuid);

            await _service.Launch.DeleteAsync(launch.Id);
        }

        [Fact]
        public async Task BigAsyncRealTreeWithEmptySuites()
        {
            var launchScheduler = new LaunchReporterBuilder(_service);
            var launchReporter = launchScheduler.Build(10, 0, 0);

            launchReporter.Sync();

            var launch = await _service.Launch.GetAsync(launchReporter.LaunchInfo.Uuid);

            await _service.Launch.DeleteAsync(launch.Id);
        }

        [Fact]
        public async Task UseExistingLaunchId()
        {
            var launchDateTime = DateTime.UtcNow;

            var launch = await _service.Launch.StartAsync(new StartLaunchRequest
            {
                Name = "UseExistingLaunchId",
                StartTime = launchDateTime,
                Mode = LaunchMode.Debug
            });

            var config = new Configuration.ConfigurationBuilder().Build();
            config.Values["Launch:Id"] = launch.Uuid;

            var launchReporter = new LaunchReporter(_service, config, null);
            launchReporter.Start(new StartLaunchRequest
            {
                Name = "SomeOtherName",
                StartTime = launchDateTime.AddDays(1)
            });
            launchReporter.Finish(new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            launchReporter.Sync();

            Assert.Equal(launch.Uuid, launchReporter.LaunchInfo.Uuid);
            Assert.Equal(launchDateTime.ToString(), launchReporter.LaunchInfo.StartTime.ToString());

            var reportedLaunch = await _service.Launch.GetAsync(launch.Uuid);
            Assert.Equal("UseExistingLaunchId", reportedLaunch.Name);
            Assert.Equal(launchDateTime.ToString(), reportedLaunch.StartTime.ToString());

            await _service.Launch.FinishAsync(launch.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var gotLaunch = await _service.Launch.GetAsync(launchReporter.LaunchInfo.Uuid);

            await _service.Launch.DeleteAsync(gotLaunch.Id);
        }

        [Fact]
        public void FinishingNonStartedLaunch()
        {
            var launchReporter = new LaunchReporter(_service, null, null);

            Assert.Throws<InsufficientExecutionStackException>(() => launchReporter.Finish(new FinishLaunchRequest()));
        }

        [Fact]
        public void StartingAlreadyStartedLaunch()
        {
            var launchReporter = new LaunchReporter(_service, null, null);
            launchReporter.Start(new StartLaunchRequest());

            Assert.Throws<InsufficientExecutionStackException>(() => launchReporter.Start(new StartLaunchRequest()));
        }

        [Fact]
        public void FinishingAlreadyFinishedLaunch()
        {
            var launchReporter = new LaunchReporter(_service, null, null);
            launchReporter.Start(new StartLaunchRequest());
            launchReporter.Finish(new FinishLaunchRequest());

            Assert.Throws<InsufficientExecutionStackException>(() => launchReporter.Finish(new FinishLaunchRequest()));
        }

        [Fact]
        public void BridgeLogMessage()
        {
            var launchReporter = new LaunchReporter(_service, null, null);

            Bridge.Context.LaunchReporter = launchReporter;

            launchReporter.Start(new StartLaunchRequest
            {
                Name = "ReportPortal Shared",
                StartTime = DateTime.UtcNow,
                Mode = LaunchMode.Debug,
                Tags = new System.Collections.Generic.List<string>()
            });


            var suiteNode = launchReporter.StartChildTestReporter(new StartTestItemRequest
            {
                Name = $"Suite",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite
            });

            var testNode = suiteNode.StartChildTestReporter(new StartTestItemRequest
            {
                Name = $"Test",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            for (int i = 0; i < 20; i++)
            {
                Log.Message(new CreateLogItemRequest
                {
                    Level = LogLevel.Info,
                    Time = DateTime.UtcNow.AddMilliseconds(i),
                    Text = $"Log {i}"
                });
            }

            testNode.Finish(new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            suiteNode.Finish(new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            launchReporter.Finish(new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            launchReporter.FinishTask.Wait();
        }
    }
}
