using System;
using System.Threading.Tasks;
using ReportPortal.Client.Abstractions.Requests;

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
                LaunchUuid = (await Service.Launch.StartAsync(new StartLaunchRequest
                {
                    Name = "StartFinishDeleteLaunch",
                    StartTime = DateTime.UtcNow
                })).Uuid;
                LaunchId = (await Service.Launch.GetAsync(LaunchUuid)).Id;
            }).Wait();
         }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await Service.Launch.StopAsync(LaunchId, new FinishLaunchRequest { EndTime = DateTime.UtcNow });
                await Service.Launch.DeleteAsync(LaunchId);
            }).Wait();
        }
    }
}
