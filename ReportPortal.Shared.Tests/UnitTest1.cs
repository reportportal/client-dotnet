using ReportPortal.Client;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var service = new Service(new Uri("https://rp.epam.com/api/v1/"), "default_project", "7853c7a9-7f27-43ea-835a-cab01355fd17");
            var launchReporter = new LaunchReporter(service);

            launchReporter.Start(new Client.Requests.StartLaunchRequest
            {
                Name = "ReportPortal.Shared",
                StartTime = DateTime.UtcNow,
                Mode = Client.Models.LaunchMode.Debug
            });

            var suiteReporter = launchReporter.StartNewTestNode(new Client.Requests.StartTestItemRequest
            {
                Name = "Suite",
                StartTime = DateTime.UtcNow,
                Type = Client.Models.TestItemType.Suite
            });

            var testReporter = suiteReporter.StartNewTestNode(new Client.Requests.StartTestItemRequest
            {
                Name = "Test",
                StartTime = DateTime.UtcNow,
                Type = Client.Models.TestItemType.Step
            });

            for (int i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1);
                testReporter.Log(new Client.Requests.AddLogItemRequest
                {
                    Text = $"Log message #{i}",
                    Time = DateTime.UtcNow,
                    Level = Client.Models.LogLevel.Info
                });
            }

            testReporter.Finish(new Client.Requests.FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Client.Models.Status.Passed
            });

            suiteReporter.Finish(new Client.Requests.FinishTestItemRequest
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
