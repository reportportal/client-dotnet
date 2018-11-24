using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Client.Tests;

namespace ReportPortal.Client.Tests
{
    public class LaunchFixtureBase: BaseFixture, IDisposable
    {
        public Launch Launch { get; set; }

        public LaunchFixtureBase()
        {
            Launch = Task.Run(async () => await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            })).Result;
        }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await Service.FinishLaunchAsync(Launch.Id, new FinishLaunchRequest { EndTime = DateTime.UtcNow }, true);
                await Service.DeleteLaunchAsync(Launch.Id);
            }).Wait();
        }
    }
}
