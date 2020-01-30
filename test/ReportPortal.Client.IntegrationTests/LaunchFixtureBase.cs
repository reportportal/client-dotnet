using System;
using System.Threading.Tasks;
using ReportPortal.Client.Requests;

namespace ReportPortal.Client.IntegrationTests
{
    public class LaunchFixtureBase : BaseFixture, IDisposable
    {
        public long LaunchId { get; set; }
        public string LaunchUuid { get; set; }

        public LaunchFixtureBase()
        {
            Task.Run(async () =>
            {
                LaunchUuid = (await Service.StartLaunchAsync(new StartLaunchRequest
                {
                    Name = "StartFinishDeleteLaunch",
                    StartTime = DateTime.UtcNow
                })).Uuid;
                LaunchId = (await Service.GetLaunchAsync(LaunchUuid)).Id;
            }).Wait();
         }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await Service.StopLaunchAsync(LaunchId, new FinishLaunchRequest { EndTime = DateTime.UtcNow });
                await Service.DeleteLaunchAsync(LaunchId);
            }).Wait();
        }
    }
}
