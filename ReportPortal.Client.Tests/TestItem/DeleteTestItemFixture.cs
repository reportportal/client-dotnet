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
                LaunchId = launch.Id,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            await Service.FinishLaunchAsync(launch.Id, new FinishLaunchRequest {EndTime = DateTime.UtcNow});

            var deleteMessage = await Service.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully deleted", deleteMessage.Info);

            await Service.DeleteLaunchAsync(launch.Id);
        }
    }
}