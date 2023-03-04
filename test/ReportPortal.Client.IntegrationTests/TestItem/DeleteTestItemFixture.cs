using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.TestItem
{
    public class DeleteTestItemFixture : IClassFixture<BaseFixture>
    {
        Service Service { get; }

        public DeleteTestItemFixture(BaseFixture baseFixture)
        {
            Service = baseFixture.Service;
        }

        [Fact]
        public async Task StartFinishDeleteTest()
        {
            var launch = await Service.Launch.StartAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteTest",
                StartTime = DateTime.UtcNow
            });

            var test = await Service.TestItem.StartAsync(new StartTestItemRequest
            {
                LaunchUuid = launch.Uuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            await Service.TestItem.FinishAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            await Service.Launch.FinishAsync(launch.Uuid, new FinishLaunchRequest { EndTime = DateTime.UtcNow });

            var tempTestItem = await Service.TestItem.GetAsync(test.Uuid);

            var deleteMessage = await Service.TestItem.DeleteAsync(tempTestItem.Id);
            Assert.Contains("successfully deleted", deleteMessage.Info);

            var tempLaunch = await Service.Launch.GetAsync(launch.Uuid);
            await Service.Launch.DeleteAsync(tempLaunch.Id);
        }
    }
}