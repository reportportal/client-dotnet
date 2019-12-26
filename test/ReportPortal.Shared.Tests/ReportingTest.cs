using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Tests.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    public class ReportingTest
    {
        private Service _service = new Service(new Uri("https://rp.epam.com/api/v1/"), "ci-agents-checks", "ba7eb7c8-7b33-42f6-8cf0-e9cd26e717f4");

        [Fact]
        public async Task BigAsyncRealTree()
        {
            var launchScheduler = new LaunchReporterBuilder(_service);
            var launchReporter = launchScheduler.Build(10, 3, 1);

            launchReporter.FinishTask.Wait();

            await _service.DeleteLaunchAsync(launchReporter.LaunchInfo.Id);
        }

        [Fact]
        public async Task BigAsyncRealTreeWithEmptySuites()
        {
            var launchScheduler = new LaunchReporterBuilder(_service);
            var launchReporter = launchScheduler.Build(10, 0, 0);

            launchReporter.FinishTask.Wait();

            await _service.DeleteLaunchAsync(launchReporter.LaunchInfo.Id);
        }

        [Fact]
        public async Task UseExistingLaunchId()
        {
            var launchDateTime = DateTime.UtcNow;

            var launch = await _service.StartLaunchAsync(new Client.Requests.StartLaunchRequest
            {
                Name = "UseExistingLaunchId",
                StartTime = launchDateTime,
                Mode = Client.Models.LaunchMode.Debug
            });

            var config = new Configuration.ConfigurationBuilder().Build();
            config.Values["Server:Launch:Id"] = launch.Id;

            var launchReporter = new LaunchReporter(_service, config, null);
            launchReporter.Start(new Client.Requests.StartLaunchRequest
            {
                Name = "SomeOtherName",
                StartTime = launchDateTime.AddDays(1)
            });
            launchReporter.Finish(new Client.Requests.FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            launchReporter.FinishTask.Wait();

            Assert.Equal(launch.Id, launchReporter.LaunchInfo.Id);
            Assert.Equal(launchDateTime.ToString(), launchReporter.LaunchInfo.StartTime.ToString());

            var reportedLaunch = await _service.GetLaunchAsync(launch.Id);
            Assert.Equal("UseExistingLaunchId", reportedLaunch.Name);
            Assert.Equal(launchDateTime.ToString(), reportedLaunch.StartTime.ToString());

            await _service.FinishLaunchAsync(launch.Id, new Client.Requests.FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            await _service.DeleteLaunchAsync(launch.Id);
        }

        [Fact]
        public void FinishingNonStartedLaunch()
        {
            var launchReporter = new LaunchReporter(_service, null, null);

            Assert.Throws<InsufficientExecutionStackException>(() => launchReporter.Finish(new Client.Requests.FinishLaunchRequest()));
        }

        [Fact]
        public void StartingAlreadyStartedLaunch()
        {
            var launchReporter = new LaunchReporter(_service, null, null);
            launchReporter.Start(new Client.Requests.StartLaunchRequest());

            Assert.Throws<InsufficientExecutionStackException>(() => launchReporter.Start(new Client.Requests.StartLaunchRequest()));
        }

        [Fact]
        public void FinishingAlreadyFinishedLaunch()
        {
            var launchReporter = new LaunchReporter(_service, null, null);
            launchReporter.Start(new Client.Requests.StartLaunchRequest());
            launchReporter.Finish(new Client.Requests.FinishLaunchRequest());

            Assert.Throws<InsufficientExecutionStackException>(() => launchReporter.Finish(new Client.Requests.FinishLaunchRequest()));
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
                Log.Message(new AddLogItemRequest
                {
                    Level = LogLevel.Info,
                    Time = DateTime.UtcNow.AddMilliseconds(i),
                    Text = $"Log {i}"
                });
            }

            testNode.Finish(new Client.Requests.FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Client.Models.Status.Passed
            });

            suiteNode.Finish(new Client.Requests.FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Client.Models.Status.Passed
            });

            launchReporter.Finish(new Client.Requests.FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            launchReporter.FinishTask.Wait();
        }
    }
}
