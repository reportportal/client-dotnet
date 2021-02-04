using ReportPortal.Client.Abstractions.Requests;
using System;

namespace ReportPortal.Client.IntegrationTests
{
    public class LaunchFixtureBase : BaseFixture, IDisposable
    {
        public long LaunchId { get; set; }
        public string LaunchUuid { get; set; }

        public LaunchFixtureBase()
        {
            LaunchUuid = Service.Launch.StartAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            }).GetAwaiter().GetResult().Uuid;

            LaunchId = Service.Launch.GetAsync(LaunchUuid).GetAwaiter().GetResult().Id;
        }

        public void Dispose()
        {
            Service.Launch.StopAsync(LaunchId, new FinishLaunchRequest { EndTime = DateTime.UtcNow }).GetAwaiter().GetResult();
            Service.Launch.DeleteAsync(LaunchId).GetAwaiter().GetResult();
        }
    }
}
