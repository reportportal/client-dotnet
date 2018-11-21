using System;
using System.Threading.Tasks;
using ReportPortal.Client.Requests;

namespace ReportPortal.Client.Tests.TestItem
{
    public class TestItemFixtureBase : BaseFixture, IDisposable
    {
        public string LaunchId { get; set; }

        public TestItemFixtureBase()
        {
            LaunchId = Task.Run(async () => await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            })).Result.Id;
        }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await Service.FinishLaunchAsync(LaunchId, new FinishLaunchRequest { EndTime = DateTime.UtcNow }, true);
                await Service.DeleteLaunchAsync(LaunchId);
            }).Wait();
        }
    }
}