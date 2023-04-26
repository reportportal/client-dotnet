using ReportPortal.Client.Abstractions.Requests;
using System;
using System.Threading.Tasks;

namespace ReportPortal.Client.IntegrationTests
{
    public class LaunchFixtureBase : BaseFixture, IDisposable
    {
        public long LaunchId { get; set; }
        public string LaunchUuid { get; set; }

        public LaunchFixtureBase()
        {
            LaunchUuid = Task.Run(async () => await Service.Launch.StartAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            })).GetAwaiter().GetResult().Uuid;

            LaunchId = Task.Run(async () => await Service.Launch.GetAsync(LaunchUuid)).GetAwaiter().GetResult().Id;
        }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await Service.Launch.StopAsync(LaunchId, new FinishLaunchRequest { EndTime = DateTime.UtcNow });
                await Service.Launch.DeleteAsync(LaunchId);
            }).GetAwaiter().GetResult();
        }
    }
}
