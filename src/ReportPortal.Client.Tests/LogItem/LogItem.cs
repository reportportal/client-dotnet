using System;
using System.Collections.Generic;
using System.Linq;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ReportPortal.Client.Tests.LogItem
{
    [Parallelizable]
    public class LogItemFixture : BaseFixture
    {
        private string _launchId;
        private string _testId;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _launchId = (await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            })).Id;

            _testId = (await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            })).Id;
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await Service.FinishTestItemAsync(_testId, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            await Service.DeleteTestItemAsync(_testId);

            await Service.DeleteLaunchAsync(_launchId);
        }

        [TestCase(LogLevel.Debug)]
        [TestCase(LogLevel.Error)]
        [TestCase(LogLevel.Info)]
        [TestCase(LogLevel.Trace)]
        [TestCase(LogLevel.Warning)]
        public async Task CteateLogWithAllLevels(LogLevel level)
        {
            var now = DateTime.UtcNow;
            var log = await Service.AddLogItemAsync(new AddLogItemRequest
            {
                TestItemId = _testId,
                Text = "Log1",
                Time = now,
                Level = level
            });
            Assert.NotNull(log.Id);
            var getLog = await Service.GetLogItemAsync(log.Id);
            Assert.AreEqual("Log1", getLog.Text);
            Assert.AreEqual(now.ToString(), getLog.Time.ToString());
        }

        [Test]
        public async Task CreateLogWithAttach()
        {
            var data = new byte[] { 1, 2, 3 };
            var log = await Service.AddLogItemAsync(new AddLogItemRequest
            {
                TestItemId = _testId,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info,
                Attach = new Attach("file1", "application/octet-stream", data)
            });
            Assert.NotNull(log.Id);
            var getLog = await Service.GetLogItemAsync(log.Id);
            Assert.AreEqual("Log1", getLog.Text);

            var logMessage = await Service.GetLogItemAsync(log.Id);
            var binaryId = logMessage.Content.Id;
            var logData = await Service.GetBinaryDataAsync(binaryId);
            CollectionAssert.AreEqual(data, logData);
        }

        [Test]
        public async Task DeleteLogItem()
        {
            var newTestId = (await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test2",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            })).Id;

            var log = await Service.AddLogItemAsync(new AddLogItemRequest
            {
                TestItemId = newTestId,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            });
            Assert.NotNull(log.Id);

            await Service.FinishTestItemAsync(newTestId, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed

            });

            var message = (await Service.DeleteLogItemAsync(log.Id)).Info;
            StringAssert.Contains("successfully", message);
        }

        [Test]
        public async Task GetLogItems()
        {
            var newTestId = (await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test3",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            })).Id;

            var log = await Service.AddLogItemAsync(new AddLogItemRequest
            {
                TestItemId = newTestId,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            });
            Assert.NotNull(log.Id);

            await Service.FinishTestItemAsync(newTestId, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            var logs = (await Service.GetLogItemsAsync(new FilterOption
            {
                Filters = new List<Filter>
                        {
                            new Filter(FilterOperation.Equals, "item", newTestId)
                        }
            })).LogItems;
            Assert.Greater(logs.Count(), 0);

            var message = (await Service.DeleteLogItemAsync(log.Id)).Info;
            StringAssert.Contains("successfully", message);
        }
    }
}
