using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System.Threading.Tasks;
using System.Net.Http;

namespace ReportPortal.Client.Tests.LaunchItem
{
    [TestFixture]
    public class LaunchItemFixture : BaseFixture
    {
        [Test]
        public async Task GetInvalidLaunch()
        {
            Assert.ThrowsAsync<HttpRequestException>(async () => await Service.GetLaunchAsync("invalid_id"));
        }

        [Test]
        public async Task GetLaunches()
        {
            var container = await Service.GetLaunchesAsync();
            var launches = container.Launches.ToList();
            Assert.Greater(launches.Count(), 0);
        }

        [Test]
        public async Task GetDebugLaunches()
        {
            var launches = await Service.GetLaunchesAsync(debug: true);
            launches.Launches.ForEach((l) => Assert.That(l.Mode, Is.EqualTo(LaunchMode.Debug)));
        }

        [Test]
        public async Task GetTheFirst10Launches()
        {
            var launches = await Service.GetLaunchesAsync(new FilterOption
            {
                Paging = new Paging(1, 10)
            });
            Assert.AreEqual(10, launches.Launches.Count());
        }

        [Test]
        public async Task GetLaunchesFilteredByName()
        {
            var launches = await Service.GetLaunchesAsync(new FilterOption
            {
                Paging = new Paging(1, 10),
                Filters = new List<Filter> { new Filter(FilterOperation.Contains, "name", "test") }
            });
            Assert.Greater(launches.Launches.Count(), 0);
            foreach (var launch in launches.Launches)
            {
                StringAssert.Contains("test", launch.Name.ToLower());
            }
        }

        //[Test]
        //public void StartFinishDeleteLaunch()
        //{
        //    var launch = Service.StartLaunch(new StartLaunchRequest
        //    {
        //        Name = "StartFinishDeleteLaunch",
        //        StartTime = DateTime.UtcNow
        //    });
        //    Assert.NotNull(launch.Id);
        //    var message = Service.FinishLaunch(launch.Id, new FinishLaunchRequest
        //    {
        //        EndTime = DateTime.UtcNow
        //    });
        //    StringAssert.Contains("successfully", message.Info);

        //    var gotLaunch = Service.GetLaunch(launch.Id);
        //    Assert.AreEqual("StartFinishDeleteLaunch", gotLaunch.Name);

        //    var delMessage = Service.DeleteLaunch(launch.Id);
        //    StringAssert.Contains("successfully", delMessage.Info);
        //}

        //[Test]
        //public void StartUpdateFinishDeleteLaunch()
        //{
        //    var launch = Service.StartLaunch(new StartLaunchRequest
        //    {
        //        Name = "StartFinishDeleteLaunch",
        //        StartTime = DateTime.UtcNow
        //    });

        //    var updateMessage = Service.UpdateLaunch(launch.Id, new UpdateLaunchRequest()
        //    {
        //        Description = launch.Description,
        //        Mode = launch.Mode,
        //        Tags = launch.Tags
        //    });

        //    Assert.NotNull(launch.Id);
        //    StringAssert.Contains("successfully updated", updateMessage.Info);
        //    var message = Service.FinishLaunch(launch.Id, new FinishLaunchRequest
        //    {
        //        EndTime = DateTime.UtcNow
        //    });
        //    StringAssert.Contains("successfully", message.Info);

        //    var gotLaunch = Service.GetLaunch(launch.Id);
        //    Assert.AreEqual("StartFinishDeleteLaunch", gotLaunch.Name);

        //    var delMessage = Service.DeleteLaunch(launch.Id);
        //    StringAssert.Contains("successfully", delMessage.Info);
        //}

        //[Test]
        //public void StartFinishDeleteFullLaunch()
        //{
        //    var now = DateTime.UtcNow;
        //    var launch = Service.StartLaunch(new StartLaunchRequest
        //    {
        //        Name = "StartFinishDeleteFullLaunch",
        //        Description = "Desc",
        //        StartTime = now,
        //        Tags = new List<string> { "tag1", "tag2", "tag3" },
        //    });
        //    Assert.NotNull(launch.Id);
        //    var getLaunch = Service.GetLaunch(launch.Id);
        //    Assert.AreEqual("StartFinishDeleteFullLaunch", getLaunch.Name);
        //    Assert.AreEqual("Desc", getLaunch.Description);
        //    Assert.AreEqual(now.ToString(), getLaunch.StartTime.ToString());
        //    CollectionAssert.AreEquivalent(new List<string> { "tag1", "tag2", "tag3" }, getLaunch.Tags);
        //    var message = Service.FinishLaunch(launch.Id, new FinishLaunchRequest
        //    {
        //        EndTime = DateTime.UtcNow
        //    });
        //    StringAssert.Contains("successfully", message.Info);
        //    var delMessage = Service.DeleteLaunch(launch.Id);
        //    StringAssert.Contains("successfully", delMessage.Info);
        //}

        //[Test]
        //public void StartFinishDeleteMergedLaunch()
        //{
        //    var launch1 = Service.StartLaunch(new StartLaunchRequest
        //    {
        //        Name = "StartFinishDeleteLaunch",
        //        StartTime = DateTime.UtcNow
        //    });
        //    Assert.NotNull(launch1.Id);
        //    var message = Service.FinishLaunch(launch1.Id, new FinishLaunchRequest
        //    {
        //        EndTime = DateTime.UtcNow
        //    });
        //    StringAssert.Contains("successfully", message.Info);

        //    var launch2 = Service.StartLaunch(new StartLaunchRequest
        //    {
        //        Name = "StartFinishDeleteLaunch2",
        //        StartTime = DateTime.UtcNow
        //    });
        //    Assert.NotNull(launch2.Id);
        //    message = Service.FinishLaunch(launch2.Id, new FinishLaunchRequest
        //    {
        //        EndTime = DateTime.UtcNow
        //    });
        //    StringAssert.Contains("successfully", message.Info);

        //    var mergedLaunch = Service.MergeLaunches(new MergeLaunchesRequest
        //    {
        //        Name = "MergedLaunch",
        //        Launches = new List<string> { launch1.Id, launch2.Id },
        //        MergeType = "BASIC",
        //        StartTime = DateTime.UtcNow,
        //        EndTime = DateTime.UtcNow
        //    });

        //    var delMessage = Service.DeleteLaunch(mergedLaunch.Id);
        //    StringAssert.Contains("successfully", delMessage.Info);
        //}

        //[Test]
        //public void StartFinishAnalyzeDeleteLaunch()
        //{
        //    var launch = Service.StartLaunch(new StartLaunchRequest
        //    {
        //        Name = "StartFinishDeleteLaunch",
        //        StartTime = DateTime.UtcNow
        //    });
        //    Assert.NotNull(launch.Id);
        //    var message = Service.FinishLaunch(launch.Id, new FinishLaunchRequest
        //    {
        //        EndTime = DateTime.UtcNow
        //    });
        //    StringAssert.Contains("successfully", message.Info);

        //    var gotLaunch = Service.GetLaunch(launch.Id);
        //    Assert.AreEqual("StartFinishDeleteLaunch", gotLaunch.Name);

        //    var analyzeMessage = Service.AnalyzeLaunch(launch.Id, "history");
        //    StringAssert.Contains("started", analyzeMessage.Info);

        //    var delMessage = Service.DeleteLaunch(launch.Id);
        //    StringAssert.Contains("successfully", delMessage.Info);
        //}

        //[Test]
        //public void TrimLaunchName()
        //{
        //    var namePrefix = "TrimLaunch";
        //    var launchName = namePrefix + new string('_', 256 - namePrefix.Length + 1);

        //    var launch = Service.StartLaunch(new StartLaunchRequest
        //    {
        //        Name = launchName,
        //        StartTime = DateTime.UtcNow
        //    });
        //    Assert.NotNull(launch.Id);
        //    var message = Service.FinishLaunch(launch.Id, new FinishLaunchRequest
        //    {
        //        EndTime = DateTime.UtcNow
        //    });
        //    StringAssert.Contains("successfully", message.Info);

        //    var gotLaunch = Service.GetLaunch(launch.Id);
        //    Assert.AreEqual(launchName.Substring(0, 256), gotLaunch.Name);

        //    var delMessage = Service.DeleteLaunch(launch.Id);
        //    StringAssert.Contains("successfully", delMessage.Info);
        //}
    }
}
