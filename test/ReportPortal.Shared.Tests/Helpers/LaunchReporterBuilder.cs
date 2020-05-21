using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Internal.Delegating;
using ReportPortal.Shared.Reporter;
using System;

namespace ReportPortal.Shared.Tests.Helpers
{
    public class LaunchReporterBuilder
    {
        public LaunchReporterBuilder(IClientService service)
        {
            Service = service;
        }

        public IClientService Service { get; }

        public IRequestExecuterFactory RequestExecuterFactory { get; set; }

        public IExtensionManager ExtensionManager { get; set; } = new ExtensionManager();

        public LaunchReporterBuilder With(IRequestExecuterFactory requestExecuterFactory)
        {
            RequestExecuterFactory = requestExecuterFactory;

            return this;
        }

        public LaunchReporterBuilder With(IExtensionManager extensionManager)
        {
            ExtensionManager = extensionManager;

            return this;
        }

        public LaunchReporter Build(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var launchReporter = new LaunchReporter(Service, null, RequestExecuterFactory?.Create(), ExtensionManager);

            var launchDateTime = DateTime.UtcNow;

            launchReporter.Start(new StartLaunchRequest
            {
                Name = "ReportPortal Shared",
                StartTime = launchDateTime,
                Mode = LaunchMode.Debug
            });

            for (int i = 0; i < suitesPerLaunch; i++)
            {
                var suiteNode = launchReporter.StartChildTestReporter(new StartTestItemRequest
                {
                    Name = $"Suite {i}",
                    StartTime = launchDateTime.AddMilliseconds(-1),
                    Type = TestItemType.Suite
                });

                for (int j = 0; j < testsPerSuite; j++)
                {
                    var testNode = suiteNode.StartChildTestReporter(new StartTestItemRequest
                    {
                        Name = $"Test {j}",
                        StartTime = launchDateTime,
                        Type = TestItemType.Step
                    });

                    for (int l = 0; l < logsPerTest; l++)
                    {
                        testNode.Log(new CreateLogItemRequest
                        {
                            Level = LogLevel.Info,
                            Text = $"Log message #{l}",
                            Time = launchDateTime
                        });
                    }

                    testNode.Finish(new FinishTestItemRequest
                    {
                        EndTime = launchDateTime,
                        Status = Status.Passed
                    });
                }

                suiteNode.Finish(new FinishTestItemRequest
                {
                    EndTime = launchDateTime,
                    Status = Status.Passed
                });
            }

            launchReporter.Finish(new FinishLaunchRequest
            {
                EndTime = launchDateTime
            });

            return launchReporter;
        }
    }
}
