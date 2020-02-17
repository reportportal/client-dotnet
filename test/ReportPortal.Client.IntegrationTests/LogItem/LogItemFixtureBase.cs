using System;
using System.Threading.Tasks;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;

namespace ReportPortal.Client.IntegrationTests.LogItem
{
    public class LogItemFixtureBase : BaseFixture, IDisposable
    {
        public string LaunchUuid { get; set; }
        public long LaunchId { get; set; }

        public string TestUuid { get; set; }
        public long TestId { get; set; }

        public LogItemFixtureBase()
        {
            Task.Run(async () =>
            {
                LaunchUuid = (await Service.Launch.StartAsync(new StartLaunchRequest
                {
                    Name = "StartFinishDeleteLaunch",
                    StartTime = DateTime.UtcNow
                })).Uuid;

                LaunchId = (await Service.Launch.GetAsync(LaunchUuid)).Id;

                TestUuid = (await Service.TestItem.StartAsync(new StartTestItemRequest
                {
                    LaunchUuid = LaunchUuid,
                    Name = "Test1",
                    StartTime = DateTime.UtcNow,
                    Type = TestItemType.Test
                })).Uuid;

                TestId = (await Service.TestItem.GetAsync(TestUuid)).Id;
            }).Wait();
        }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await Service.Launch.FinishAsync(LaunchUuid, new FinishLaunchRequest { EndTime = DateTime.UtcNow });
                await Service.Launch.DeleteAsync(LaunchId);
            }).Wait();
        }
    }
}