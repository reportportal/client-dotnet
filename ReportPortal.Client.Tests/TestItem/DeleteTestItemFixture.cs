using System;
using System.Threading.Tasks;
using Xunit;
using ReportPortal.Client.Api.Launch.Requests;
using ReportPortal.Client.Api.TestItem.Model;
using ReportPortal.Client.Api.TestItem.Request;

namespace ReportPortal.Client.Tests.TestItem
{
    public class DeleteTestItemFixture : BaseFixture
    {
        [Fact]
        public async Task StartFinishDeleteTest()
        {
            var launch = await Service.Launch.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteTest",
                StartTime = DateTime.UtcNow
            });

            var test = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = launch.Id,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            await Service.Launch.FinishLaunchAsync(launch.Id, new FinishLaunchRequest {EndTime = DateTime.UtcNow});

            var deleteMessage = await Service.TestItem.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully deleted", deleteMessage.Info);

            await Service.Launch.DeleteLaunchAsync(launch.Id);
        }
    }
}