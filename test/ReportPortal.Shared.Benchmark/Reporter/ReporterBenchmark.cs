using BenchmarkDotNet.Attributes;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Reporter;
using System;

namespace ReportPortal.Shared.Benchmark.Reporter
{
    [MemoryDiagnoser]
    [SimpleJob(targetCount: 10)]
    public class ReporterBenchmark
    {
        [Params(1, 100000)]
        public int SuitesCount { get; set; }

        [Benchmark]
        public void LaunchReporter()
        {
            var configuration = new Configuration.ConfigurationBuilder().Build();

            var nopService = new NopService();
            var launchReporter = new LaunchReporter(nopService, configuration, null);

            var launchDateTime = DateTime.UtcNow;

            launchReporter.Start(new StartLaunchRequest
            {
                Name = "ReportPortal Benchmark",
                StartTime = launchDateTime,
                Mode = LaunchMode.Debug
            });

            for (int i = 0; i < SuitesCount; i++)
            {
                var suiteNode = launchReporter.StartChildTestReporter(new StartTestItemRequest
                {
                    Name = $"Suite {i}",
                    StartTime = launchDateTime.AddMilliseconds(-1),
                    Type = TestItemType.Suite
                });

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

            launchReporter.Sync();
        }
    }
}
