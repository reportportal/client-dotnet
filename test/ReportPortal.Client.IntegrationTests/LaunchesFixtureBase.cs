using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportPortal.Client.IntegrationTests
{
    public class LaunchesFixtureBase : BaseFixture, IDisposable
    {
        public List<LaunchCreatedResponse> CreatedLaunches { get; } = new List<LaunchCreatedResponse>();

        public LaunchesFixtureBase()
        {
            for (int i = 0; i < 10; i++)
            {
                var createdLaunch = Task.Run(async () => await Service.Launch.StartAsync(new StartLaunchRequest
                {
                    Name = "LaunchItemFixture",
                    StartTime = DateTime.UtcNow,
                    Mode = LaunchMode.Default
                })).GetAwaiter().GetResult();

                Task.Run(async () => await Service.Launch.FinishAsync(createdLaunch.Uuid, new FinishLaunchRequest
                {
                    EndTime = DateTime.UtcNow
                })).GetAwaiter().GetResult();

                CreatedLaunches.Add(createdLaunch);
            }
        }

        public void Dispose()
        {
            foreach (var createdLaunch in CreatedLaunches)
            {
                Task.Run(async () =>
                {
                    var launch = await Service.Launch.GetAsync(createdLaunch.Uuid);
                    await Service.Launch.DeleteAsync(launch.Id);

                }).GetAwaiter().GetResult();
            }
        }
    }
}
