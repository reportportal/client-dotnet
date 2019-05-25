using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Tests.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    public class ReportingTest
    {
        private Service _service = new Service(new Uri("https://rp.epam.com/api/v1/"), "ci-agents-checks", "b79e81a5-8448-49b5-857d-945ff5fd5ed2");

        [Fact]
        public async Task BigAsyncRealTree()
        {
            var launchScheduler = new LaunchScheduler(_service);
            var launchReporter = launchScheduler.Build(10, 3, 1);

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

            var launchReporter = new LaunchReporter(_service, launch.Id);
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
            var launchReporter = new LaunchReporter(_service);

            Assert.Throws<InsufficientExecutionStackException>(() => launchReporter.Finish(new Client.Requests.FinishLaunchRequest()));
        }

        [Fact]
        public void StartingAlreadyStartedLaunch()
        {
            var launchReporter = new LaunchReporter(_service);
            launchReporter.Start(new Client.Requests.StartLaunchRequest());

            Assert.Throws<InsufficientExecutionStackException>(() => launchReporter.Start(new Client.Requests.StartLaunchRequest()));
        }

        [Fact]
        public void FinishingAlreadyFinishedLaunch()
        {
            var launchReporter = new LaunchReporter(_service);
            launchReporter.Start(new Client.Requests.StartLaunchRequest());
            launchReporter.Finish(new Client.Requests.FinishLaunchRequest());

            Assert.Throws<InsufficientExecutionStackException>(() => launchReporter.Finish(new Client.Requests.FinishLaunchRequest()));
        }
    }
}
