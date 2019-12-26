using BenchmarkDotNet.Attributes;
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
            var nopService = new NopService(new Uri("https://rp.epam.com/api/v1/"), "", "");
            var launchReporter = new LaunchReporter(nopService, null, null);

            var launchDateTime = DateTime.UtcNow;

            launchReporter.Start(new Client.Requests.StartLaunchRequest
            {
                Name = "ReportPortal Benchmark",
                StartTime = launchDateTime,
                Mode = Client.Models.LaunchMode.Debug,
                Tags = new System.Collections.Generic.List<string>()
            });

            for (int i = 0; i < SuitesCount; i++)
            {
                var suiteNode = launchReporter.StartChildTestReporter(new Client.Requests.StartTestItemRequest
                {
                    Name = $"Suite {i}",
                    StartTime = launchDateTime.AddMilliseconds(-1),
                    Type = Client.Models.TestItemType.Suite
                });

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

            launchReporter.Sync();
        }
    }
}
