using FluentAssertions;
using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.LaunchItem
{
    public class GetLaunchesItemFixture : IClassFixture<LaunchesFixtureBase>, IClassFixture<BaseFixture>
    {
        Service Service { get; }

        public GetLaunchesItemFixture(BaseFixture baseFixture)
        {
            Service = baseFixture.Service;
        }

        [Fact]
        public async Task GetInvalidLaunch()
        {
            await Assert.ThrowsAsync<ServiceException>(async () => await Service.Launch.GetAsync("invalid_id"));
        }

        [Fact]
        public async Task GetLaunches()
        {
            var container = await Service.Launch.GetAsync();
            var launches = container.Items.ToList();
            Assert.True(launches.Count() > 0);
        }

        [Fact]
        public async Task GetDebugLaunches()
        {
            var launches = await Service.Launch.GetDebugAsync();
            launches.Items.ToList().ForEach((l) => Assert.Equal(LaunchMode.Debug, l.Mode));
        }

        [Fact]
        public async Task GetTheFirst10Launches()
        {
            var launches = await Service.Launch.GetAsync(new FilterOption
            {
                Paging = new Paging(1, 10)
            });
            Assert.Equal(10, launches.Items.Count());
        }

        [Fact]
        public async Task GetLaunchesFilteredByName()
        {
            var launches = await Service.Launch.GetAsync(new FilterOption
            {
                Paging = new Paging(1, 10),
                Filters = new List<Filter> { new Filter(FilterOperation.Contains, "name", "LaunchItemFixture") }
            });

            launches.Items.Should().HaveCount(10);

            foreach (var launch in launches.Items)
            {
                Assert.Contains("LaunchItemFixture", launch.Name);
            }
        }

        [Fact]
        public async Task GetLaunchesSortedByAscendingDate()
        {
            var launches = await Service.Launch.GetAsync(new FilterOption
            {
                Paging = new Paging(1, 10),
                Sorting = new Sorting(new List<string> { "startTime" }, SortDirection.Ascending)
            });

            Assert.True(launches.Items.Count() > 0);

            Assert.Equal(launches.Items.Select(l => l.StartTime).OrderBy(st => st), launches.Items.Select(l => l.StartTime));
        }

        [Fact]
        public async Task GetLaunchesSortedByDescendingDate()
        {
            var launches = await Service.Launch.GetAsync(new FilterOption
            {
                Paging = new Paging(1, 10),
                Sorting = new Sorting(new List<string> { "startTime" }, SortDirection.Descending)
            });

            Assert.True(launches.Items.Count() > 0);

            Assert.Equal(launches.Items.Select(l => l.StartTime).OrderByDescending(st => st), launches.Items.Select(l => l.StartTime));
        }

        [Fact]
        public async Task GetInProgressLaunch()
        {
            var launch = await Service.Launch.StartAsync(new StartLaunchRequest
            {
                Name = "StartForceFinishIncompleteLaunch",
                StartTime = DateTime.UtcNow,
                Mode = LaunchMode.Default
            });

            var getLaunch = await Service.Launch.GetAsync(launch.Uuid);

            Assert.NotEqual(default(DateTime), getLaunch.StartTime);
            Assert.Null(getLaunch.EndTime);

            await Service.Launch.FinishAsync(launch.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var delMessage = await Service.Launch.DeleteAsync(getLaunch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }
        
        [Fact]
        public async Task GetLatestLaunches()
        {
            var container = await Service.Launch.GetAsync();
            var uniqueLaunches = container.Items
              .OrderByDescending(l => l.StartTime)
              .GroupBy(l => l.Name)
              .Select(g => g.First())
              .ToList();

            Assert.True(uniqueLaunches.Count > 0);
            
            var latestContainer = await Service.Launch.GetLatestAsync();
            var latestLaunches = latestContainer.Items.OrderByDescending(l => l.StartTime).ToList();
            
            Assert.Equal(uniqueLaunches.Count, latestLaunches.Count);
            Assert.Multiple(() =>
            {
              for (int i = 0; i < latestLaunches.Count; i++)
              {
                  Assert.Equal(uniqueLaunches[i].Name, latestLaunches[i].Name);
              }
            });
        }

        [Fact]
        public async Task GetLatestLaunchesWithMultipleItems()
        {
            var launch = await Service.Launch.StartAsync(new StartLaunchRequest
            {
              Name = $"StartForceFinishIncompleteLaunch {DateTime.UtcNow.Ticks}",
              StartTime = DateTime.UtcNow,
              Mode = LaunchMode.Default
            });

            var getLaunch = await Service.Launch.GetAsync(launch.Uuid);

            Assert.NotEqual(default(DateTime), getLaunch.StartTime);
            Assert.Null(getLaunch.EndTime);

            await Service.Launch.FinishAsync(launch.Uuid, new FinishLaunchRequest
            {
              EndTime = DateTime.UtcNow
            });

            var latestLaunches = await Service.Launch.GetLatestAsync();
            Assert.Contains(latestLaunches.Items, l => l.Name == getLaunch.Name && l.Number == 1);

            var delMessage = await Service.Launch.DeleteAsync(getLaunch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task GetLatestLaunchesFilteredByName()
        {
            var launch = await Service.Launch.StartAsync(new StartLaunchRequest
            {
              Name = $"AnotherStartForceFinishIncompleteLaunch  {DateTime.UtcNow.Ticks}",
              StartTime = DateTime.UtcNow,
              Mode = LaunchMode.Default
            });

            var getLaunch = await Service.Launch.GetAsync(launch.Uuid);

            Assert.NotEqual(default(DateTime), getLaunch.StartTime);
            Assert.Null(getLaunch.EndTime);

            await Service.Launch.FinishAsync(launch.Uuid, new FinishLaunchRequest
            {
              EndTime = DateTime.UtcNow
            });

            var latestLaunches = await Service.Launch.GetLatestAsync(new FilterOption
            {
              Paging = new Paging(1, 10),
              Filters = new List<Filter> { new Filter(FilterOperation.Equals, "name", getLaunch.Name) }
            });
            
            Assert.Single(latestLaunches.Items);
            Assert.Equal(1, latestLaunches.Items[0].Number);

            var delMessage = await Service.Launch.DeleteAsync(getLaunch.Id);
            Assert.Contains("successfully", delMessage.Info);
        }
    }
}
