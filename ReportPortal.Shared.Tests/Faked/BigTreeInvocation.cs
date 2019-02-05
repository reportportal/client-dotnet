using ReportPortal.Shared.Reporter;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ReportPortal.Shared.Tests.Faked
{
    public class BigTreeInvocation
    {
        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(5, 10000, 0)]
        [InlineData(5, 10000, 10)]
        public void MockedBigTree(int suitesCount, int testsCount, int logsCount)
        {
            var fakeService = new FakeService(new Uri("https://rp.epam.com/api/v1/"), "ci-agents-checks", "7853c7a9-7f27-43ea-835a-cab01355fd17");
            var launchReporter = new LaunchReporter(fakeService);

            var launchDateTime = DateTime.UtcNow;

            launchReporter.Start(new Client.Requests.StartLaunchRequest
            {
                Name = "ReportPortal Shared",
                StartTime = launchDateTime,
                Mode = Client.Models.LaunchMode.Debug,
                Tags = new System.Collections.Generic.List<string>()
            });

            for (int i = 0; i < suitesCount; i++)
            {
                var suiteNode = launchReporter.StartChildTestReporter(new Client.Requests.StartTestItemRequest
                {
                    Name = $"Suite {i}",
                    StartTime = launchDateTime.AddMilliseconds(-1),
                    Type = Client.Models.TestItemType.Suite
                });

                for (int j = 0; j < testsCount; j++)
                {
                    var testNode = suiteNode.StartChildTestReporter(new Client.Requests.StartTestItemRequest
                    {
                        Name = $"Test {j}",
                        StartTime = launchDateTime,
                        Type = Client.Models.TestItemType.Step
                    });

                    for (int l = 0; l < logsCount; l++)
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

            Assert.Equal(suitesCount * testsCount + suitesCount, fakeService.StartTestItemCounter);

            Assert.Equal(suitesCount * testsCount * logsCount, fakeService.AddLogItemCounter);
        }
    }
}
