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
            var launch = await Service.LaunchClient.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteTest",
                StartTime = DateTime.UtcNow
            });

            var test = await Service.TestItemClient.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = launch.Id,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            await Service.TestItemClient.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            await Service.LaunchClient.FinishLaunchAsync(launch.Id, new FinishLaunchRequest {EndTime = DateTime.UtcNow});

            var deleteMessage = await Service.TestItemClient.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully deleted", deleteMessage.Info);

            await Service.LaunchClient.DeleteLaunchAsync(launch.Id);
        }
    }
}