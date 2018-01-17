using System;
using System.Collections.Generic;
using System.Linq;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System.Threading.Tasks;
using System.Net.Http;
using Xunit;

namespace ReportPortal.Client.Tests.LaunchItem
{
    [Trait("LaunchItem", "")]
    public class LaunchItemFixture : BaseFixture
    {
        [Fact]
        public async Task GetInvalidLaunch()
        {
            await Assert.ThrowsAsync<HttpRequestException>(async () => await Service.GetLaunchAsync("invalid_id"));
        }

        [Fact]
        public async Task GetLaunches()
        {
            var container = await Service.GetLaunchesAsync();
            var launches = container.Launches.ToList();
            Assert.True(launches.Count() > 0);
        }

        [Fact]
        public async Task GetDebugLaunches()
        {
            var launches = await Service.GetLaunchesAsync(debug: true);
            launches.Launches.ForEach((l) => Assert.Equal(LaunchMode.Debug, l.Mode));
        }

        [Fact]
        public async Task GetTheFirst10Launches()
        {
            var launches = await Service.GetLaunchesAsync(new FilterOption
            {
                Paging = new Paging(1, 10)
            });
            Assert.Equal(10, launches.Launches.Count());
        }

        [Fact]
        public async Task GetLaunchesFilteredByName()
        {
            var launches = await Service.GetLaunchesAsync(new FilterOption
            {
                Paging = new Paging(1, 10),
                Filters = new List<Filter> { new Filter(FilterOperation.Contains, "name", "test") }
            });
            Assert.True(launches.Launches.Count() > 0);
            foreach (var launch in launches.Launches)
            {
                Assert.Contains("test", launch.Name.ToLower());
            }
        }

        [Fact]
        public async Task StartFinishDeleteLaunch()
        {
            var launch = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            });
            Assert.NotNull(launch.Id);
            var message = await Service.FinishLaunchAsync(launch.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Contains("successfully", message.Info);

            var gotLaunch = await Service.GetLaunchAsync(launch.Id);
            Assert.Equal("StartFinishDeleteLaunch", gotLaunch.Name);

            var delMessage = await Service.DeleteLaunchAsync(launch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task StartUpdateFinishDeleteLaunch()
        {
            var launch = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            });

            var updateMessage = await Service.UpdateLaunchAsync(launch.Id, new UpdateLaunchRequest()
            {
                Description = launch.Description,
                Mode = launch.Mode,
                Tags = launch.Tags
            });

            Assert.NotNull(launch.Id);
            Assert.Contains("successfully updated", updateMessage.Info);
            var message = await Service.FinishLaunchAsync(launch.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Contains("successfully", message.Info);

            var gotLaunch = await Service.GetLaunchAsync(launch.Id);
            Assert.Equal("StartFinishDeleteLaunch", gotLaunch.Name);

            var delMessage = await Service.DeleteLaunchAsync(launch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task StartFinishDeleteFullLaunch()
        {
            var now = DateTime.UtcNow;
            var launch = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteFullLaunch",
                Description = "Desc",
                StartTime = now,
                Tags = new List<string> { "tag1", "tag2", "tag3" },
            });
            Assert.NotNull(launch.Id);
            var getLaunch = await Service.GetLaunchAsync(launch.Id);
            Assert.Equal("StartFinishDeleteFullLaunch", getLaunch.Name);
            Assert.Equal("Desc", getLaunch.Description);
            Assert.Equal(now.ToString(), getLaunch.StartTime.ToString());
            Assert.Equal(new List<string> { "tag1", "tag2", "tag3" }, getLaunch.Tags);
            var message = await Service.FinishLaunchAsync(launch.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Contains("successfully", message.Info);
            var delMessage = await Service.DeleteLaunchAsync(launch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task StartFinishDeleteMergedLaunch()
        {
            var launch1 = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            });
            Assert.NotNull(launch1.Id);
            var message = await Service.FinishLaunchAsync(launch1.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Contains("successfully", message.Info);

            var launch2 = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch2",
                StartTime = DateTime.UtcNow
            });
            Assert.NotNull(launch2.Id);
            message = await Service.FinishLaunchAsync(launch2.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Contains("successfully", message.Info);

            var mergedLaunch = await Service.MergeLaunchesAsync(new MergeLaunchesRequest
            {
                Name = "MergedLaunch",
                Launches = new List<string> { launch1.Id, launch2.Id },
                MergeType = "BASIC",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow
            });

            var delMessage = await Service.DeleteLaunchAsync(mergedLaunch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task StartFinishAnalyzeDeleteLaunch()
        {
            var launch = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            });
            Assert.NotNull(launch.Id);
            var message = await Service.FinishLaunchAsync(launch.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Contains("successfully", message.Info);

            var gotLaunch = await Service.GetLaunchAsync(launch.Id);
            Assert.Equal("StartFinishDeleteLaunch", gotLaunch.Name);

            var analyzeMessage = await Service.AnalyzeLaunchAsync(launch.Id, "history");
            Assert.Contains("started", analyzeMessage.Info);

            var delMessage = await Service.DeleteLaunchAsync(launch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task TrimLaunchName()
        {
            var namePrefix = "TrimLaunch";
            var launchName = namePrefix + new string('_', 256 - namePrefix.Length + 1);

            var launch = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = launchName,
                StartTime = DateTime.UtcNow
            });
            Assert.NotNull(launch.Id);
            var message = await Service.FinishLaunchAsync(launch.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Contains("successfully", message.Info);

            var gotLaunch = await Service.GetLaunchAsync(launch.Id);
            Assert.Equal(launchName.Substring(0, 256), gotLaunch.Name);

            var delMessage = await Service.DeleteLaunchAsync(launch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }
    }
}
