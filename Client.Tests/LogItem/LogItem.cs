using System;
using System.Collections.Generic;
using System.Linq;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using NUnit.Framework;

namespace ReportPortal.Client.Tests.LogItem
{
    public class LogItemFixture: BaseFixture
    {
        private string _launchId;
        private string _testId;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _launchId = Service.StartLaunch(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            }).Id;

            _testId = Service.StartTestItem(new StartTestItemRequest
                {
                    LaunchId = _launchId,
                    Name = "Test1",
                    StartTime = DateTime.UtcNow,
                    Type = TestItemType.Test
                }).Id;
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Service.FinishTestItem(_testId, new FinishTestItemRequest
                {
                    EndTime = DateTime.UtcNow,
                    Status = Status.Passed
                });

            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            Service.DeleteTestItem(_testId);

            Service.DeleteLaunch(_launchId);
        }

        [TestCase(LogLevel.Debug)]
        [TestCase(LogLevel.Error)]
        [TestCase(LogLevel.Info)]
        [TestCase(LogLevel.Trace)]
        [TestCase(LogLevel.Warning)]
        public void CteateLogWithAllLevels(LogLevel level)
        {
            var now = DateTime.UtcNow;
            var log = Service.AddLogItem(new AddLogItemRequest
            {
                TestItemId = _testId,
                Text = "Log1",
                Time = now,
                Level = level
            });
            Assert.NotNull(log.Id);
            var getLog = Service.GetLogItem(log.Id);
            Assert.AreEqual("Log1", getLog.Text);
            Assert.AreEqual(now.ToString(), getLog.Time.ToString());
        }

        [Test]
        public void CteateLogWithAttach()
        {
            var data = new byte[]{1, 2, 3};
            var log = Service.AddLogItem(new AddLogItemRequest
            {
                TestItemId = _testId,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info,
                Attach = new Attach("file1", "application/octet-stream", data)
            });
            Assert.NotNull(log.Id);
            var getLog = Service.GetLogItem(log.Id);
            Assert.AreEqual("Log1", getLog.Text);

            var logMessage = Service.GetLogItem(log.Id);
            var binaryId = logMessage.Content.Id;
            var logData = Service.GetBinaryData(binaryId);
            CollectionAssert.AreEqual(data, logData);
        }

        [Test]
        public void DeleteLogItem()
        {
            var newTestId = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test2",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            }).Id;

            var log = Service.AddLogItem(new AddLogItemRequest
            {
                TestItemId = newTestId,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            });
            Assert.NotNull(log.Id);

            Service.FinishTestItem(newTestId, new FinishTestItemRequest
                {
                    EndTime = DateTime.UtcNow,
                    Status = Status.Passed

                });

            var message = Service.DeleteLogItem(log.Id).Info;
            StringAssert.Contains("successfully", message);
        }

        [Test]
        public void GetLogItems()
        {
            var newTestId = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test3",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            }).Id;

            var log = Service.AddLogItem(new AddLogItemRequest
            {
                TestItemId = newTestId,
                Text = "Log1",
                Time = DateTime.UtcNow,
                Level = LogLevel.Info
            });
            Assert.NotNull(log.Id);

            Service.FinishTestItem(newTestId, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            var logs = Service.GetLogItems(new FilterOption
                {
                    Filters = new List<Filter>
                        {
                            new Filter(FilterOperation.Equals, "item", newTestId)
                        }
                });
            Assert.Greater(logs.Count(), 0);

            var message = Service.DeleteLogItem(log.Id).Info;
            StringAssert.Contains("successfully", message);
        }
    }
}
