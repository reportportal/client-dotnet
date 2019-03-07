using System;
using System.Threading.Tasks;
using ReportPortal.Client.Api.Launch.Request;
using ReportPortal.Client.Api.TestItem.Model;
using ReportPortal.Client.Api.TestItem.Request;

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
                LaunchId = (await Service.Launch.StartLaunchAsync(new StartLaunchRequest
                {
                    Name = "StartFinishDeleteLaunch",
                    StartTime = DateTime.UtcNow
                })).Id;

                TestId = (await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
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
                await Service.Launch.FinishLaunchAsync(LaunchId, new FinishLaunchRequest { EndTime = DateTime.UtcNow }, true);
                await Service.Launch.DeleteLaunchAsync(LaunchId);
            }).Wait();
        }
    }
}