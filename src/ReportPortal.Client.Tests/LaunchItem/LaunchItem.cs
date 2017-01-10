using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Requests;

namespace ReportPortal.Client.Tests.LaunchItem
{
    [TestFixture]
    public class LaunchItemFixture : BaseFixture
    {
        [Test]
        public void GetInvalidLaunch()
        {
            Assert.Throws<ServiceException>(() => Service.GetLaunch("invalid_id"));
        }

        [Test]
        public void GetLaunches()
        {
            var launches = Service.GetLaunches().Launches.ToList();
            Assert.Greater(launches.Count(), 0);
        }

        [Test]
        public void GetTheFirst10Launches()
        {
            var launches = Service.GetLaunches(new FilterOption
                {
                    Paging = new Paging(1, 10)
                });
            Assert.AreEqual(10, launches.Launches.Count());
        }

        [Test]
        public void GetLaunchesFilteredByName()
        {
            var launches = Service.GetLaunches(new FilterOption
            {
                Paging = new Paging(1, 10),
                Filters = new List<Filter> { new Filter(FilterOperation.Contains, "name", "test") }
            }).Launches;
            Assert.Greater(launches.Count(), 0);
            foreach (var launch in launches)
            {
                StringAssert.Contains("test", launch.Name.ToLower());
            }
        }

        [Test]
        public void StartFinishDeleteLaunch()
        {
            var launch = Service.StartLaunch(new StartLaunchRequest
                {
                    Name = "StartFinishDeleteLaunch",
                    StartTime = DateTime.UtcNow
                });
            Assert.NotNull(launch.Id);
            var message = Service.FinishLaunch(launch.Id, new FinishLaunchRequest
                {
                    EndTime = DateTime.UtcNow
                });
            StringAssert.Contains("successfully", message.Info);

            var gotLaunch = Service.GetLaunch(launch.Id);
            Assert.AreEqual("StartFinishDeleteLaunch", gotLaunch.Name);

            var delMessage = Service.DeleteLaunch(launch.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [Test]
        public void StartUpdateFinishDeleteLaunch()
        {
            var launch = Service.StartLaunch(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            });

            var updateMessage = Service.UpdateLaunch(launch.Id, new UpdateLaunchRequest()
            {
                Description = launch.Description,
                Mode = launch.Mode,
                Tags = launch.Tags
            });

            Assert.NotNull(launch.Id);
            StringAssert.Contains("successfully updated", updateMessage.Info);
            var message = Service.FinishLaunch(launch.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            StringAssert.Contains("successfully", message.Info);

            var gotLaunch = Service.GetLaunch(launch.Id);
            Assert.AreEqual("StartFinishDeleteLaunch", gotLaunch.Name);

            var delMessage = Service.DeleteLaunch(launch.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [Test]
        public void StartFinishDeleteFullLaunch()
        {
            var now = DateTime.UtcNow;
            var launch = Service.StartLaunch(new StartLaunchRequest
            {
                Name = "StartFinishDeleteFullLaunch",
                Description = "Desc",
                StartTime = now,
                Tags = new List<string> { "tag1", "tag2", "tag3" },
            });
            Assert.NotNull(launch.Id);
            var getLaunch = Service.GetLaunch(launch.Id);
            Assert.AreEqual("StartFinishDeleteFullLaunch", getLaunch.Name);
            Assert.AreEqual("Desc", getLaunch.Description);
            Assert.AreEqual(now.ToString(), getLaunch.StartTime.ToString());
            CollectionAssert.AreEquivalent(new List<string> { "tag1", "tag2", "tag3" }, getLaunch.Tags);
            var message = Service.FinishLaunch(launch.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            StringAssert.Contains("successfully", message.Info);
            var delMessage = Service.DeleteLaunch(launch.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [Test]
        public void StartFinishDeleteMergedLaunch()
        {
            var launch1 = Service.StartLaunch(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            });
            Assert.NotNull(launch1.Id);
            var message = Service.FinishLaunch(launch1.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            StringAssert.Contains("successfully", message.Info);

            var launch2 = Service.StartLaunch(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch2",
                StartTime = DateTime.UtcNow
            });
            Assert.NotNull(launch2.Id);
            message = Service.FinishLaunch(launch2.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            StringAssert.Contains("successfully", message.Info);

            var mergedLaunch = Service.MergeLaunches(new MergeLaunchesRequest()
            {
                Name = "MergedLaunch",
                Launches = new List<string> { launch1.Id, launch2.Id}
            });

            var delMessage = Service.DeleteLaunch(mergedLaunch.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [Test]
        public void StartFinishAnalyzeDeleteLaunch()
        {
            var launch = Service.StartLaunch(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            });
            Assert.NotNull(launch.Id);
            var message = Service.FinishLaunch(launch.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            StringAssert.Contains("successfully", message.Info);

            var gotLaunch = Service.GetLaunch(launch.Id);
            Assert.AreEqual("StartFinishDeleteLaunch", gotLaunch.Name);

            var analyzeMessage = Service.AnalyzeLaunch(launch.Id, "history");
            StringAssert.Contains("started", analyzeMessage.Info);

            var delMessage = Service.DeleteLaunch(launch.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [Test]
        public void TrimLaunchName()
        {
            var namePrefix = "TrimLaunch";
            var launchName = namePrefix + new string('_', 256 - namePrefix.Length + 1);

            var launch = Service.StartLaunch(new StartLaunchRequest
            {
                Name = launchName,
                StartTime = DateTime.UtcNow
            });
            Assert.NotNull(launch.Id);
            var message = Service.FinishLaunch(launch.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            StringAssert.Contains("successfully", message.Info);

            var gotLaunch = Service.GetLaunch(launch.Id);
            Assert.AreEqual(launchName.Substring(0, 256), gotLaunch.Name);

            var delMessage = Service.DeleteLaunch(launch.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }
    }
}
