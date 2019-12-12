using ReportPortal.Client;
using ReportPortal.Shared.Internal.Delegating;
using ReportPortal.Shared.Reporter;
using System;

namespace ReportPortal.Shared.Tests.Helpers
{
    public class LaunchReporterBuilder
    {
        public LaunchReporterBuilder(Service service)
        {
            Service = service;
        }

        public Service Service { get; }

        public IRequestExecuterFactory RequestExecuterFactory { get; set; }

        public LaunchReporterBuilder With(IRequestExecuterFactory requestExecuterFactory)
        {
            RequestExecuterFactory = requestExecuterFactory;

            return this;
        }

        public LaunchReporter Build(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var launchReporter = new LaunchReporter(Service, null, RequestExecuterFactory);

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

            return launchReporter;
        }
    }
}
