using FluentAssertions;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.TestItem
{
    public class UpdateTestItemFixture : IClassFixture<LaunchFixtureBase>, IClassFixture<BaseFixture>
    {
        private LaunchFixtureBase _fixture;

        Service Service { get; }

        public UpdateTestItemFixture(LaunchFixtureBase fixture, BaseFixture baseFixture)
        {
            _fixture = fixture;

            Service = baseFixture.Service;
        }

        [Fact]
        public async Task UpdateDescription()
        {
            var originalRequest = new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "UpdateDescription",
                StartTime = DateTime.UtcNow
            };

            var test = await Service.TestItem.StartAsync(originalRequest);

            var tempTest = await Service.TestItem.GetAsync(test.Uuid);

            var updateRequest = new UpdateTestItemRequest()
            {
                Description = "newDesc",
            };

            var updateMessage = await Service.TestItem.UpdateAsync(tempTest.Id, updateRequest);

            updateMessage.Info.Should().Contain("successfully");

            var updatedTest = await Service.TestItem.GetAsync(test.Uuid);
            updatedTest.Name.Should().Be(originalRequest.Name);
            updatedTest.Description.Should().Be(updateRequest.Description);

            await Service.TestItem.FinishAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
        }

        [Fact]
        public async Task UpdateAttributes()
        {
            var originalRequest = new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "UpdateAttributes",
                StartTime = DateTime.UtcNow
            };

            var test = await Service.TestItem.StartAsync(originalRequest);

            var tempTest = await Service.TestItem.GetAsync(test.Uuid);

            var updateRequest = new UpdateTestItemRequest()
            {
                Attributes = new List<ItemAttribute> { new ItemAttribute { Key = "k1", Value = "v1" } },
            };

            var updateMessage = await Service.TestItem.UpdateAsync(tempTest.Id, updateRequest);

            updateMessage.Info.Should().Contain("successfully");

            var updatedTest = await Service.TestItem.GetAsync(test.Uuid);
            updatedTest.Name.Should().Be(originalRequest.Name);
            updatedTest.Attributes.Should().BeEquivalentTo(updateRequest.Attributes);

            await Service.TestItem.FinishAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
        }

        [Fact]
        public async Task UpdateOfInProgressIsNotAllowed()
        {
            var originalRequest = new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "UpdateAttributes",
                StartTime = DateTime.UtcNow
            };

            var test = await Service.TestItem.StartAsync(originalRequest);

            var tempTest = await Service.TestItem.GetAsync(test.Uuid);

            var updateRequest = new UpdateTestItemRequest()
            {
                Status = Status.Passed
            };

            Func<Task> act = async () => await Service.TestItem.UpdateAsync(tempTest.Id, updateRequest);

            var exp = await act.Should().ThrowExactlyAsync<ServiceException>();
            exp.Which.ResponseBody.Should().Contain("Unable to update status");

            await Service.TestItem.FinishAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
        }

        [Theory]
        [InlineData(Status.Failed)]
        [InlineData(Status.Passed)]
        [InlineData(Status.Skipped)]
        public async Task UpdateStatus(Status status)
        {
            var originalRequest = new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "UpdateAttributes",
                StartTime = DateTime.UtcNow
            };

            var test = await Service.TestItem.StartAsync(originalRequest);

            await Service.TestItem.FinishAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            var tempTest = await Service.TestItem.GetAsync(test.Uuid);

            var updateRequest = new UpdateTestItemRequest()
            {
                Status = status
            };

            await Service.TestItem.UpdateAsync(tempTest.Id, updateRequest);

            var updatedTest = await Service.TestItem.GetAsync(test.Uuid);
            updatedTest.Name.Should().Be(originalRequest.Name);
            updatedTest.Description.Should().Be(originalRequest.Description);

            updatedTest.Status.Should().Be(status);
        }
    }
}