using System;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace ReportPortal.Client.IntegrationTests.LogItem
{
    public class LogItemFixtureBase : BaseFixture, IDisposable
    {
        public string LaunchUuid { get; set; }
        public long LaunchId { get; set; }

        public string TestUuid { get; set; }

        public LogItemFixtureBase()
        {
            Task.Run(async () =>
            {
                LaunchUuid = (await Service.StartLaunchAsync(new StartLaunchRequest
                {
                    Name = "StartFinishDeleteLaunch",
                    StartTime = DateTime.UtcNow
                })).Uuid;

                LaunchId = (await Service.GetLaunchAsync(LaunchUuid)).Id;

                TestUuid = (await Service.StartTestItemAsync(new StartTestItemRequest
                {
                    LaunchUuid = LaunchUuid,
                    Name = "Test1",
                    StartTime = DateTime.UtcNow,
                    Type = TestItemType.Test
                })).Uuid;
            }).Wait();
        }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await Service.FinishLaunchAsync(LaunchUuid, new FinishLaunchRequest { EndTime = DateTime.UtcNow });
                await Service.DeleteLaunchAsync(LaunchId);
            }).Wait();
        }
    }
}