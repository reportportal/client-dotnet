using System;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.Tests.TestItem
{
    public class DeleteTestItemFixture : BaseFixture
    {
        [Fact]
        public async Task StartFinishDeleteTest()
        {
            var launch = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteTest",
                StartTime = DateTime.UtcNow
            });

            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = launch.Uuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            await Service.FinishLaunchAsync(launch.Uuid, new FinishLaunchRequest { EndTime = DateTime.UtcNow });

            var tempTestItem = await Service.GetTestItemAsync(test.Uuid);

            var deleteMessage = await Service.DeleteTestItemAsync(tempTestItem.Id);
            Assert.Contains("successfully deleted", deleteMessage.Info);

            var tempLaunch = await Service.GetLaunchAsync(launch.Uuid);
            await Service.DeleteLaunchAsync(tempLaunch.Id);
        }
    }
}