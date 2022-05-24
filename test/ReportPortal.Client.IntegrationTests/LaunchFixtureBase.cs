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
            LaunchUuid = Task.Run(() => Service.Launch.StartAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            })).GetAwaiter().GetResult().Uuid;

            LaunchId = Task.Run(() => Service.Launch.GetAsync(LaunchUuid)).GetAwaiter().GetResult().Id;
        }

        public void Dispose()
        {
            Task.Run(() => Service.Launch.StopAsync(LaunchId, new FinishLaunchRequest { EndTime = DateTime.UtcNow })).Wait();
            Task.Run(() => Service.Launch.DeleteAsync(LaunchId)).Wait();
        }
    }
}
