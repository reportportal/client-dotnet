using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Text;
using ReportPortal.Client.Api.TestItem.Model;
using ReportPortal.Client.Api.TestItem.Request;
using ReportPortal.Client.Api.Log.Model;
using ReportPortal.Client.Api.Log.Request;
using ReportPortal.Client.Common.Model.Filtering;

namespace ReportPortal.Client.Tests.LogItem
{
    public class LogItemFixture : BaseFixture, IClassFixture<LogItemFixtureBase>
    {
        private LogItemFixtureBase _fixture;
        public LogItemFixture(LogItemFixtureBase fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Error)]
        [InlineData(LogLevel.Fatal)]
        [InlineData(LogLevel.Info)]
        [InlineData(LogLevel.Trace)]
        [InlineData(LogLevel.Warning)]
        public async Task CteateLogWithAllLevels(LogLevel level)
        {
            var now = DateTime.UtcNow;
            var log = await Service.Log.AddLogItemAsync(new AddLogItemRequest
            {
                TestItemId = _fixture.TestId,
                Text = "Log1",
                Time = now,
                Level = level
            });
            Assert.NotNull(log.Id);
            var getLog = await Service.Log.GetLogItemAsync(log.Id);
            Assert.Equal("Log1", getLog.Text);
            Assert.Equal(now.ToString(), getLog.Time.ToString());
        }

        [Fact]
        public async Task CreateLogWithAttach()
        {
            var data = new byte[] { 1, 2, 3 };
            var log = await Service.Log.AddLogItemAsync(new AddLogItemRequest
            {
                TestItemId = _fixture.TestId,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info,
                Attach = new Attach("file1", "application/octet-stream", data)
            });
            Assert.NotNull(log.Id);
            var getLog = await Service.Log.GetLogItemAsync(log.Id);
            Assert.Equal("Log1", getLog.Text);

            var logMessage = await Service.Log.GetLogItemAsync(log.Id);
            var binaryId = logMessage.Content.Id;
            var logData = await Service.Log.GetBinaryDataAsync(binaryId);
            Assert.Equal(data, logData);
        }

        [Fact]
        public async Task CreateLogWithJsonAttach()
        {
            var data = Encoding.Default.GetBytes("{\"a\" = true }");
            var log = await Service.Log.AddLogItemAsync(new AddLogItemRequest
            {
                TestItemId = _fixture.TestId,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info,
                Attach = new Attach("file1", "application/json", data)
            });
            Assert.NotNull(log.Id);
            var getLog = await Service.Log.GetLogItemAsync(log.Id);
            Assert.Equal("Log1", getLog.Text);

            var logMessage = await Service.Log.GetLogItemAsync(log.Id);
            var binaryId = logMessage.Content.Id;
            var logData = await Service.Log.GetBinaryDataAsync(binaryId);
            Assert.Equal(data, logData);
        }

        [Fact]
        public async Task DeleteLogItem()
        {
            var newTestId = (await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test2",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            })).Id;

            var log = await Service.Log.AddLogItemAsync(new AddLogItemRequest
            {
                TestItemId = newTestId,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            });
            Assert.NotNull(log.Id);

            await Service.TestItem.FinishTestItemAsync(newTestId, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed

            });

            var message = (await Service.Log.DeleteLogItemAsync(log.Id)).Info;
            Assert.Contains("successfully", message);
        }

        [Fact]
        public async Task GetLogItem()
        {
            var addLogItemRequest = new AddLogItemRequest
            {
                TestItemId = _fixture.TestId,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            };

            var log = await Service.Log.AddLogItemAsync(addLogItemRequest);
            Assert.NotNull(log.Id);

            var gotLogItem = await Service.Log.GetLogItemAsync(log.Id);
            Assert.Equal(addLogItemRequest.Text, gotLogItem.Text);
            Assert.Equal(addLogItemRequest.Level, gotLogItem.Level);
            Assert.Equal(addLogItemRequest.Time, gotLogItem.Time);
        }

        [Fact]
        public async Task GetLogItems()
        {
            var newTestId = (await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test3",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            })).Id;

            var log = await Service.Log.AddLogItemAsync(new AddLogItemRequest
            {
                TestItemId = newTestId,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            });
            Assert.NotNull(log.Id);

            await Service.TestItem.FinishTestItemAsync(newTestId, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            var logs = (await Service.Log.GetLogItemsAsync(new FilterOption
            {
                FilterConditions = new List<FilterCondition>
                        {
                            new FilterCondition(FilterOperation.Equals, "item", newTestId)
                        }
            })).Collection;
            Assert.True(logs.Count() > 0);

            var message = (await Service.Log.DeleteLogItemAsync(log.Id)).Info;
            Assert.Contains("successfully", message);
        }
    }
}
