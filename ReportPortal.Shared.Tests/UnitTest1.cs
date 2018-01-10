using ReportPortal.Client;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void OneHundredLogMessages()
        {
            var service = new Service(new Uri("https://rp.epam.com/api/v1/"), "default_project", "7853c7a9-7f27-43ea-835a-cab01355fd17");
            var launchReporter = new LaunchReporter(service);
            launchReporter.Start(new Client.Requests.StartLaunchRequest
            {
                Name = "ReportPortal Shared",
                StartTime = DateTime.UtcNow,
                Mode = Client.Models.LaunchMode.Debug,
                Tags = new System.Collections.Generic.List<string>()
            });

            var suiteNode = launchReporter.StartNewTestNode(new Client.Requests.StartTestItemRequest
            {
                Name = "Suite",
                StartTime = DateTime.UtcNow,
                Type = Client.Models.TestItemType.Suite
            });

            var testNode = suiteNode.StartNewTestNode(new Client.Requests.StartTestItemRequest
            {
                Name = "Test",
                StartTime = DateTime.UtcNow,
                Type = Client.Models.TestItemType.Step
            });

            for (int i = 0; i < 10; i++)
            {
                testNode.Log(new Client.Requests.AddLogItemRequest
                {
                    Level = Client.Models.LogLevel.Info,
                    Text = $"Log message #{i}",
                    Time = DateTime.UtcNow
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
