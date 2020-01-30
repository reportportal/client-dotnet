using System;
using System.Collections.Generic;
using System.Linq;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System.Threading.Tasks;
using System.Net.Http;
using Xunit;

namespace ReportPortal.Client.Tests.LaunchItem
{
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
        public async Task GetLaunchesSortedByAscendingDate()
        {
            var launches = await Service.GetLaunchesAsync(new FilterOption
            {
                Paging = new Paging(1, 10),
                Sorting = new Sorting(new List<string> { "startTime" }, SortDirection.Ascending)
            });

            Assert.True(launches.Launches.Count() > 0);

            Assert.Equal(launches.Launches.Select(l => l.StartTime).OrderBy(st => st), launches.Launches.Select(l => l.StartTime));
        }

        [Fact]
        public async Task GetLaunchesSortedByDescendingDate()
        {
            var launches = await Service.GetLaunchesAsync(new FilterOption
            {
                Paging = new Paging(1, 10),
                Sorting = new Sorting(new List<string> { "startTime" }, SortDirection.Descending)
            });

            Assert.True(launches.Launches.Count() > 0);

            Assert.Equal(launches.Launches.Select(l => l.StartTime).OrderByDescending(st => st), launches.Launches.Select(l => l.StartTime));
        }

        [Fact]
        public async Task StartFinishDeleteLaunch()
        {
            var startLaunchRequest = new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            };

            var launch = await Service.StartLaunchAsync(startLaunchRequest);
            Assert.NotNull(launch.Uuid);

            var finishLaunchRequest = new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow.AddHours(1)
            };

            var message = await Service.FinishLaunchAsync(launch.Uuid, finishLaunchRequest);
            Assert.Equal(launch.Uuid, message.Id);

            var gotLaunch = await Service.GetLaunchAsync(launch.Uuid);
            Assert.Equal("StartFinishDeleteLaunch", gotLaunch.Name);
            Assert.Equal(startLaunchRequest.StartTime, gotLaunch.StartTime);
            Assert.Equal(finishLaunchRequest.EndTime, gotLaunch.EndTime);

            var delMessage = await Service.DeleteLaunchAsync(gotLaunch.Id);
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

            var tempLaunch = await Service.GetLaunchAsync(launch.Uuid);

            var updateMessage = await Service.UpdateLaunchAsync(tempLaunch.Id, new UpdateLaunchRequest()
            {
                Description = "New description",
                Mode = LaunchMode.Debug,
                Tags = null
            });

            Assert.NotNull(launch.Uuid);
            Assert.Contains("successfully updated", updateMessage.Info);
            var message = await Service.FinishLaunchAsync(launch.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Equal(launch.Uuid, message.Id);

            var gotLaunch = await Service.GetLaunchAsync(launch.Uuid);
            Assert.Equal("StartFinishDeleteLaunch", gotLaunch.Name);

            var delMessage = await Service.DeleteLaunchAsync(gotLaunch.Id);
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
                //Tags = new List<string> { "tag1", "tag2", "tag3" },
            });
            Assert.NotNull(launch.Uuid);
            var getLaunch = await Service.GetLaunchAsync(launch.Uuid);
            Assert.Equal("StartFinishDeleteFullLaunch", getLaunch.Name);
            Assert.Equal("Desc", getLaunch.Description);
            Assert.Equal(now.ToString(), getLaunch.StartTime.ToString());
            //Assert.Equal(new List<string> { "tag1", "tag2", "tag3" }, getLaunch.Tags);
            var message = await Service.FinishLaunchAsync(launch.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Equal(launch.Uuid, message.Id);
            var delMessage = await Service.DeleteLaunchAsync(getLaunch.Id);
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
            Assert.NotNull(launch1.Uuid);
            var message = await Service.FinishLaunchAsync(launch1.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Equal(launch1.Uuid, message.Id);

            var launch2 = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch2",
                StartTime = DateTime.UtcNow
            });
            Assert.NotNull(launch2.Uuid);
            message = await Service.FinishLaunchAsync(launch2.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Equal(launch2.Uuid, message.Id);

            var getLaunch1 = await Service.GetLaunchAsync(launch1.Uuid);
            var getLaunch2 = await Service.GetLaunchAsync(launch2.Uuid);
            var mergeRequest = new MergeLaunchesRequest
            {
                Name = "MergedLaunch",
                Launches = new List<long> { getLaunch1.Id, getLaunch2.Id },
                MergeType = "BASIC",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow
            };

            var mergedLaunch = await Service.MergeLaunchesAsync(mergeRequest);
            Assert.Equal(mergeRequest.StartTime, mergedLaunch.StartTime);
            Assert.Equal(mergeRequest.EndTime, mergedLaunch.EndTime);

            var delMessage = await Service.DeleteLaunchAsync(mergedLaunch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task StartFinishAnalyzeDeleteLaunch()
        {
            var launch = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow,
                Mode = LaunchMode.Default
            });
            Assert.NotNull(launch.Uuid);
            var message = await Service.FinishLaunchAsync(launch.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Equal(launch.Uuid, message.Id);

            var gotLaunch = await Service.GetLaunchAsync(launch.Uuid);
            Assert.Equal("StartFinishDeleteLaunch", gotLaunch.Name);

            var analyzeMessage = await Service.AnalyzeLaunchAsync(new AnalyzeLaunchRequest
            {
                LaunchId = gotLaunch.Id,
                AnalyzerMode = AnalyzerMode.LaunchName,
                AnalyzerTypeName = "autoAnalyzer",
                AnalyzerItemsMode = new List<AnalyzerItemsMode> { AnalyzerItemsMode.ToInvestigate }
            });
            Assert.Contains("started", analyzeMessage.Info);
            
            var delMessage = await Service.DeleteLaunchAsync(gotLaunch.Id);
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
            Assert.NotNull(launch.Uuid);
            var message = await Service.FinishLaunchAsync(launch.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            Assert.Contains(launch.Uuid, message.Id);

            var gotLaunch = await Service.GetLaunchAsync(launch.Uuid);
            Assert.Equal(launchName.Substring(0, 256), gotLaunch.Name);

            var delMessage = await Service.DeleteLaunchAsync(gotLaunch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task StartForceFinishIncompleteLaunch()
        {
            var launch = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartForceFinishIncompleteLaunch",
                StartTime = DateTime.UtcNow,
                Mode = LaunchMode.Default
            });

            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = launch.Uuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Uuid);

            var tempLaunch = await Service.GetLaunchAsync(launch.Uuid);

            await Service.StopLaunchAsync(tempLaunch.Id, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var getLaunch = await Service.GetLaunchAsync(launch.Uuid);

            var delMessage = await Service.DeleteLaunchAsync(getLaunch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task GetInProgressLaunch()
        {
            var launch = await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartForceFinishIncompleteLaunch",
                StartTime = DateTime.UtcNow,
                Mode = LaunchMode.Default
            });

            var getLaunch = await Service.GetLaunchAsync(launch.Uuid);

            Assert.NotEqual(default(DateTime), getLaunch.StartTime);
            Assert.Null(getLaunch.EndTime);

            await Service.FinishLaunchAsync(launch.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var delMessage = await Service.DeleteLaunchAsync(getLaunch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }
    }
}
