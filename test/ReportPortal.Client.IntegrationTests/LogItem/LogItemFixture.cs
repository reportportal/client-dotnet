using FluentAssertions;
using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.LogItem
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
            var log = await Service.LogItem.CreateAsync(new CreateLogItemRequest
            {
                TestItemUuid = _fixture.TestUuid,
                Text = "Log1",
                Time = now,
                Level = level
            });
            Assert.NotNull(log.Uuid);
            var getLog = await Service.LogItem.GetAsync(log.Uuid);
            Assert.Equal(0, getLog.LaunchId);
            Assert.Equal(_fixture.TestId, getLog.TestItemId);
            Assert.Equal("Log1", getLog.Text);
            Assert.Equal(now.ToString(), getLog.Time.ToString());
        }

        [Fact]
        public async Task CreateLogWithAttach()
        {
            var data = new byte[] { 1, 2, 3 };
            var log = await Service.LogItem.CreateAsync(new CreateLogItemRequest
            {
                TestItemUuid = _fixture.TestUuid,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info,
                Attach = new Attach("application/octet-stream", data)
            });
            Assert.NotNull(log.Uuid);
            var getLog = await Service.LogItem.GetAsync(log.Uuid);
            Assert.Equal("Log1", getLog.Text);

            var logMessage = await Service.LogItem.GetAsync(log.Uuid);
            var binaryId = logMessage.Content.Id;
            var logData = await Service.LogItem.GetBinaryDataAsync(binaryId);
            Assert.Equal(data, logData);
        }

        [Fact]
        public async Task CreateLogWithJsonAttach()
        {
            var data = Encoding.Default.GetBytes("{\"a\" = true }");
            var log = await Service.LogItem.CreateAsync(new CreateLogItemRequest
            {
                TestItemUuid = _fixture.TestUuid,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info,
                Attach = new Attach("application/json", data)
            });
            Assert.NotNull(log.Uuid);
            var getLog = await Service.LogItem.GetAsync(log.Uuid);
            Assert.Equal("Log1", getLog.Text);

            var binaryId = getLog.Content.Id;

            var logData = await Service.LogItem.GetBinaryDataAsync(binaryId);
            Assert.Equal(data, logData);
        }

        [Fact]
        public async Task CreateSeveralLogsWithAttach()
        {
            var requests = new List<CreateLogItemRequest>();

            for (int i = 0; i < 10; i++)
            {
                requests.Add(new CreateLogItemRequest
                {
                    TestItemUuid = _fixture.TestUuid,
                    Text = $"Log{i}",
                    Time = DateTime.UtcNow,
                    Level = LogLevel.Info,
                    Attach = new Attach("application/json", new byte[] { (byte)i })
                });
            }

            var logs = await Service.LogItem.CreateAsync(requests.ToArray());

            logs.LogItems.Should().HaveCount(10);

            for (int i = 0; i < 10; i++)
            {
                var log = logs.LogItems[i];
                log.Uuid.Should().NotBeNullOrEmpty();

                var getLog = await Service.LogItem.GetAsync(log.Uuid);
                Assert.Equal($"Log{i}", getLog.Text);

                var binaryId = getLog.Content.Id;
                var logData = await Service.LogItem.GetBinaryDataAsync(binaryId);
                logData.Should().BeEquivalentTo(new byte[] { (byte)i });
            }
        }

        [Fact]
        public async Task CreateLogForLaunch()
        {
            var log = await Service.LogItem.CreateAsync(new CreateLogItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Text = "LaunchLog1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            });

            var getLog = await Service.LogItem.GetAsync(log.Uuid);

            Assert.Equal(_fixture.LaunchId, getLog.LaunchId);
            Assert.Equal(0, getLog.TestItemId);
        }

        [Fact]
        public async Task CreateLogWithDefaultStartTime()
        {
            var utcNow = DateTime.UtcNow;

            var log = await Service.LogItem.CreateAsync(new CreateLogItemRequest
            {
                TestItemUuid = _fixture.TestUuid,
                Text = "TestLog",
                Level = LogLevel.Info
            });

            var getLog = await Service.LogItem.GetAsync(log.Uuid);

            getLog.Time.Should().BeCloseTo(utcNow);
        }

        [Fact]
        public async Task DeleteLogItem()
        {
            var newTestUuid = (await Service.TestItem.StartAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test2",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            })).Uuid;

            var log = await Service.LogItem.CreateAsync(new CreateLogItemRequest
            {
                TestItemUuid = newTestUuid,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            });
            Assert.NotNull(log.Uuid);

            await Service.TestItem.FinishAsync(newTestUuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed

            });

            var tempLogItem = await Service.LogItem.GetAsync(log.Uuid);

            var message = (await Service.LogItem.DeleteAsync(tempLogItem.Id)).Info;
            Assert.Contains("successfully", message);
        }

        [Fact]
        public async Task GetLogItem()
        {
            var addLogItemRequest = new CreateLogItemRequest
            {
                TestItemUuid = _fixture.TestUuid,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            };

            var log = await Service.LogItem.CreateAsync(addLogItemRequest);
            Assert.NotNull(log.Uuid);

            var gotLogItem = await Service.LogItem.GetAsync(log.Uuid);
            Assert.Equal(addLogItemRequest.Text, gotLogItem.Text);
            Assert.Equal(addLogItemRequest.Level, gotLogItem.Level);
            Assert.Equal(addLogItemRequest.Time, gotLogItem.Time);
        }

        [Fact]
        public async Task GetLogItems()
        {
            var newTestUuid = (await Service.TestItem.StartAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test3",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            })).Uuid;

            var log = await Service.LogItem.CreateAsync(new CreateLogItemRequest
            {
                TestItemUuid = newTestUuid,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            });
            Assert.NotNull(log.Uuid);

            await Service.TestItem.FinishAsync(newTestUuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            var tempTest = await Service.TestItem.GetAsync(newTestUuid);
            var gotlogs = (await Service.LogItem.GetAsync(new FilterOption
            {
                Filters = new List<Filter>
                        {
                            new Filter(FilterOperation.Equals, "item", tempTest.Id)
                        }
            })).Items;
            Assert.True(gotlogs.Count() > 0);

            var tempLogItem = await Service.LogItem.GetAsync(log.Uuid);
            var message = (await Service.LogItem.DeleteAsync(tempLogItem.Id)).Info;
            Assert.Contains("successfully", message);
        }
    }
}
