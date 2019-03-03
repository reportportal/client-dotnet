using System;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace ReportPortal.Client.Tests.LogItem
{
    public class LogItemFixtureBase : BaseFixture, IDisposable
    {
        public string LaunchId { get; set; }

        public string TestId { get; set; }

        public LogItemFixtureBase()
        {
            Task.Run(async () =>
            {
                LaunchId = (await Service.LaunchClient.StartLaunchAsync(new StartLaunchRequest
                {
                    Name = "StartFinishDeleteLaunch",
                    StartTime = DateTime.UtcNow
                })).Id;

                TestId = (await Service.TestItemClient.StartTestItemAsync(new StartTestItemRequest
                {
                    LaunchId = LaunchId,
                    Name = "Test1",
                    StartTime = DateTime.UtcNow,
                    Type = TestItemType.Test
                })).Id;
            }).Wait();
        }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await Service.LaunchClient.FinishLaunchAsync(LaunchId, new FinishLaunchRequest { EndTime = DateTime.UtcNow }, true);
                await Service.LaunchClient.DeleteLaunchAsync(LaunchId);
            }).Wait();
        }
    }
}