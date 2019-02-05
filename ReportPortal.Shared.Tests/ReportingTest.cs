using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared.Reporter;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    public class ReportingTest
    {
        private Service _service = new Service(new Uri("https://rp.epam.com/api/v1/"), "ci-agents-checks", "7853c7a9-7f27-43ea-835a-cab01355fd17");

        [Fact]
        public async Task BigAsyncRealTree()
        {
            var launchReporter = new LaunchReporter(_service);

            var launchDateTime = DateTime.UtcNow;

            launchReporter.Start(new Client.Requests.StartLaunchRequest
            {
                Name = "ReportPortal Shared",
                StartTime = launchDateTime,
                Mode = Client.Models.LaunchMode.Debug,
                Tags = new System.Collections.Generic.List<string>()
            });

            for (int i = 0; i < 10; i++)
            {
                var suiteNode = launchReporter.StartChildTestReporter(new Client.Requests.StartTestItemRequest
                {
                    Name = $"Suite {i}",
                    StartTime = launchDateTime.AddMilliseconds(-1),
                    Type = Client.Models.TestItemType.Suite
                });

                for (int j = 0; j < 3; j++)
                {
                    var testNode = suiteNode.StartChildTestReporter(new Client.Requests.StartTestItemRequest
                    {
                        Name = $"Test {j}",
                        StartTime = launchDateTime,
                        Type = Client.Models.TestItemType.Step
                    });

                    for (int l = 0; l < 0; l++)
                    {
                        testNode.Log(new Client.Requests.AddLogItemRequest
                        {
                            Level = Client.Models.LogLevel.Info,
                            Text = $"Log message #{l}",
                            Time = launchDateTime
                        });
                    }

                    testNode.Finish(new Client.Requests.FinishTestItemRequest
                    {
                        EndTime = launchDateTime,
                        Status = Client.Models.Status.Passed
                    });
                }

                suiteNode.Finish(new Client.Requests.FinishTestItemRequest
                {
                    EndTime = launchDateTime,
                    Status = Client.Models.Status.Passed
                });
            }

            launchReporter.Finish(new Client.Requests.FinishLaunchRequest
            {
                EndTime = launchDateTime
            });

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
