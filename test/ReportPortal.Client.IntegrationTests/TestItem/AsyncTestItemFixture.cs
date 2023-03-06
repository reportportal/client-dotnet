using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.TestItem
{
    public class AsyncTestItemFixture : IClassFixture<LaunchFixtureBase>, IClassFixture<BaseFixture>
    {
        private readonly LaunchFixtureBase _fixture;

        Service Service { get; }

        public AsyncTestItemFixture(LaunchFixtureBase fixture, BaseFixture baseFixture)
        {
            _fixture = fixture;
            Service = baseFixture.Service;
        }

        [Fact]
        public async Task StartFinishAsyncTest()
        {
            var test = await Service.AsyncTestItem.StartAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1_Async",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test.Uuid);

            var message = await Service.AsyncTestItem.FinishAsync(test.Uuid, new FinishTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            Assert.Contains("Accepted finish request for test item", message.Info);
        }
    }
}