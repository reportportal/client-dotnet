using ReportPortal.Shared.Reporter;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Faked
{
    public class BigTreeInvocation
    {
        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(5, 10000, 0)]
        [InlineData(5, 10000, 10)]
        public void SuccessReporting(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
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

            for (int i = 0; i < suitesPerLaunch; i++)
            {
                var suiteNode = launchReporter.StartChildTestReporter(new Client.Requests.StartTestItemRequest
                {
                    Name = $"Suite {i}",
                    StartTime = launchDateTime.AddMilliseconds(-1),
                    Type = Client.Models.TestItemType.Suite
                });

                for (int j = 0; j < testsPerSuite; j++)
                {
                    var testNode = suiteNode.StartChildTestReporter(new Client.Requests.StartTestItemRequest
                    {
                        Name = $"Test {j}",
                        StartTime = launchDateTime,
                        Type = Client.Models.TestItemType.Step
                    });

                    for (int l = 0; l < logsPerTest; l++)
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

            Assert.Equal(suitesPerLaunch * testsPerSuite + suitesPerLaunch, fakeService.StartTestItemCounter);

            Assert.Equal(suitesPerLaunch * testsPerSuite * logsPerTest, fakeService.AddLogItemCounter);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(10, 10, 10)]
        public void FailedLogsShouldNotAffectFinishingLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var fakeService = new FakeServiceWithFailedAddLogItemMethod(new Uri("https://rp.epam.com/api/v1/"), "ci-agents-checks", "7853c7a9-7f27-43ea-835a-cab01355fd17");
            var launchReporter = new LaunchReporter(fakeService);

            var launchDateTime = DateTime.UtcNow;

            launchReporter.Start(new Client.Requests.StartLaunchRequest
            {
                Name = "ReportPortal Shared",
                StartTime = launchDateTime,
                Mode = Client.Models.LaunchMode.Debug,
                Tags = new System.Collections.Generic.List<string>()
            });

            for (int i = 0; i < suitesPerLaunch; i++)
            {
                var suiteNode = launchReporter.StartChildTestReporter(new Client.Requests.StartTestItemRequest
                {
                    Name = $"Suite {i}",
                    StartTime = launchDateTime.AddMilliseconds(-1),
                    Type = Client.Models.TestItemType.Suite
                });

                for (int j = 0; j < testsPerSuite; j++)
                {
                    var testNode = suiteNode.StartChildTestReporter(new Client.Requests.StartTestItemRequest
                    {
                        Name = $"Test {j}",
                        StartTime = launchDateTime,
                        Type = Client.Models.TestItemType.Step
                    });

                    for (int l = 0; l < logsPerTest; l++)
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

            Assert.Equal(suitesPerLaunch * testsPerSuite + suitesPerLaunch, fakeService.StartTestItemCounter);

            Assert.Equal(suitesPerLaunch * testsPerSuite + suitesPerLaunch, fakeService.FinishTestItemCounter);
        }
    }
}
