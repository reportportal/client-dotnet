using FluentAssertions;
using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.LogItem
{
    public class AsyncLogItemFixture : IClassFixture<LogItemFixtureBase>, IClassFixture<BaseFixture>
    {
        private LogItemFixtureBase _fixture;

        Service Service { get; }

        public AsyncLogItemFixture(LogItemFixtureBase fixture, BaseFixture baseFixture)
        {
            _fixture = fixture;
            Service = baseFixture.Service;
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
            var log = await Service.AsyncLogItem.CreateAsync(new CreateLogItemRequest
            {
                TestItemUuid = _fixture.TestUuid,
                Text = "Log1",
                Time = now,
                Level = level
            });
            Assert.NotNull(log.Uuid);
        }

        [Fact]
        public async Task CreateLogWithAttach()
        {
            var data = new byte[] { 1, 2, 3 };
            var log = await Service.AsyncLogItem.CreateAsync(new CreateLogItemRequest
            {
                TestItemUuid = _fixture.TestUuid,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info,
                Attach = new LogItemAttach("application/octet-stream", data)
            });
            
            Assert.NotNull(log.Uuid);
        }

        [Fact]
        public async Task CreateLogWithJsonAttach()
        {
            var data = Encoding.Default.GetBytes("{\"a\" = true }");
            var log = await Service.AsyncLogItem.CreateAsync(new CreateLogItemRequest
            {
                TestItemUuid = _fixture.TestUuid,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info,
                Attach = new LogItemAttach("application/json", data)
            });
            Assert.NotNull(log.Uuid);
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
                    Attach = new LogItemAttach("application/json", new byte[] { (byte)i })
                });
            }

            var logs = await Service.AsyncLogItem.CreateAsync(requests.ToArray());

            logs.LogItems.Should().HaveCount(10);
        }

        [Fact]
        public async Task CreateLogForLaunch()
        {
            var log = await Service.AsyncLogItem.CreateAsync(new CreateLogItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Text = "LaunchLog1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            });

            Assert.NotNull(log.Uuid);
        }

        [Fact]
        public async Task CreateLogWithDefaultStartTime()
        {
            var utcNow = DateTime.UtcNow;

            var log = await Service.AsyncLogItem.CreateAsync(new CreateLogItemRequest
            {
                TestItemUuid = _fixture.TestUuid,
                Text = "TestLog",
                Level = LogLevel.Info
            });
        }
    }
}
