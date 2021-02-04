using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System;
using System.Collections.Generic;

namespace ReportPortal.Client.IntegrationTests
{
    public class LaunchesFixtureBase : BaseFixture, IDisposable
    {
        private List<LaunchCreatedResponse> CreatedLaunches { get; } = new List<LaunchCreatedResponse>();

        public LaunchesFixtureBase()
        {
            for (int i = 0; i < 10; i++)
            {
                var createdLaunch = Service.Launch.StartAsync(new StartLaunchRequest
                {
                    Name = "LaunchItemFixture",
                    StartTime = DateTime.UtcNow,
                    Mode = LaunchMode.Default
                }).GetAwaiter().GetResult();

                Service.Launch.FinishAsync(createdLaunch.Uuid, new FinishLaunchRequest
                {
                    EndTime = DateTime.UtcNow
                }).GetAwaiter().GetResult();

                CreatedLaunches.Add(createdLaunch);
            }
        }

        public void Dispose()
        {
            foreach (var createdLaunch in CreatedLaunches)
            {
                var gotCreatedLaunch = Service.Launch.GetAsync(createdLaunch.Uuid).GetAwaiter().GetResult();

                Service.Launch.DeleteAsync(gotCreatedLaunch.Id).GetAwaiter().GetResult();
            }
        }
    }
}
