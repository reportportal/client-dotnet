using FluentAssertions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.LaunchItem
{
    public class AsyncLaunchItemFixture : IClassFixture<BaseFixture>
    {
        Service Service { get; }

        public AsyncLaunchItemFixture(BaseFixture baseFixture)
        {
            Service = baseFixture.Service;
        }

        [Fact]
        public async Task StartFinishDeleteLaunch()
        {
            var startLaunchRequest = new StartLaunchRequest
            {
                Name = "StartFinishDeleteAsyncLaunch",
                StartTime = DateTime.UtcNow
            };

            var launch = await Service.AsyncLaunch.StartAsync(startLaunchRequest);
            Assert.NotNull(launch.Uuid);
            Assert.Equal(0, launch.Number);

            var finishLaunchRequest = new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow.AddHours(1)
            };

            var message = await Service.AsyncLaunch.FinishAsync(launch.Uuid, finishLaunchRequest);
            Assert.Equal(launch.Uuid, message.Uuid);

            LaunchResponse gotLaunch = null;

            // wait until async launch will be processed
            Func<Task> getLaunchAction = async () => gotLaunch = await Service.Launch.GetAsync(launch.Uuid);
            await getLaunchAction.Should().NotThrowAfterAsync(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));

            Assert.NotNull(gotLaunch);
            Assert.Equal("StartFinishDeleteAsyncLaunch", gotLaunch.Name);
            gotLaunch.StartTime.Should().BeCloseTo(startLaunchRequest.StartTime, precision: TimeSpan.FromMilliseconds(1));
            gotLaunch.EndTime.Should().BeCloseTo(finishLaunchRequest.EndTime, precision: TimeSpan.FromMilliseconds(1));

            var delMessage = await Service.Launch.DeleteAsync(gotLaunch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }
    }
}
