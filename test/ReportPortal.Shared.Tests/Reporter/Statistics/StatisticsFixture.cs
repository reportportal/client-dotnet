using FluentAssertions;
using FluentAssertions.Extensions;
using ReportPortal.Shared.Reporter.Statistics;
using ReportPortal.Shared.Tests.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests.Reporter.Statistics
{
    public class StatisticsFixture
    {
        [Fact]
        public void ShouldHaveDefaultValues()
        {
            var counter = new StatisticsCounter();

            counter.Min.Should().Be(TimeSpan.Zero);
            counter.Max.Should().Be(TimeSpan.Zero);
            counter.Avg.Should().Be(TimeSpan.Zero);
            counter.Count.Should().Be(0);
        }

        [Fact]
        public void ShouldCountRequests()
        {
            var counter = new StatisticsCounter();

            counter.Measure(TimeSpan.Zero);
            counter.Measure(TimeSpan.Zero);

            counter.Count.Should().Be(2);
        }

        [Fact]
        public void ShouldCountMinValue()
        {
            var counter = new StatisticsCounter();

            counter.Measure(1.Seconds());
            counter.Measure(2.Seconds());

            counter.Min.Should().Be(1.Seconds());
        }

        [Fact]
        public void ShouldCountMaxValue()
        {
            var counter = new StatisticsCounter();

            counter.Measure(1.Seconds());
            counter.Measure(2.Seconds());

            counter.Max.Should().Be(2.Seconds());
        }

        [Fact]
        public void ShouldCountAverageValue()
        {
            var counter = new StatisticsCounter();

            counter.Measure(1.Seconds());
            counter.Measure(2.Seconds());
            counter.Measure(3.Seconds());

            counter.Avg.Should().Be(2.Seconds());
        }

        [Fact]
        public void ShouldCountAverageFloatingValue()
        {
            var counter = new StatisticsCounter();

            counter.Measure(1.Seconds());
            counter.Measure(2.Seconds());

            counter.Avg.Should().Be(1.5.Seconds());
        }

        [Fact]
        public void ShouldCountParallelRequests()
        {
            var counter = new StatisticsCounter();

            var values = Enumerable.Range(1, 1000);

            Parallel.ForEach(values, (v) => counter.Measure(v.Seconds()));

            counter.Min.Should().Be(1.Seconds());
            counter.Max.Should().Be(1000.Seconds());
            counter.Avg.Should().Be(500.5.Seconds());
        }

        [Fact]
        public void ShouldHaveStringRepresentation()
        {
            var counter = new StatisticsCounter();
            counter.Measure(1.111.Seconds());
            counter.Measure(2.222.Seconds());
            counter.Measure(3.333.Seconds());

            counter.ToString().Should().Be("3 cnt min/avg/max 1111/2222/3333 ms");
        }

        [Fact]
        public void LaunchShouldUseStatisticsCounter()
        {
            var service = new MockServiceBuilder().Build();

            var launchScheduler = new LaunchReporterBuilder(service.Object).Build(2, 5, 10);

            launchScheduler.Sync();

            var expectedItemInvocations = 2 * 5 + 2;

            launchScheduler.StatisticsCounter.StartTestItemStatisticsCounter.Count.Should().Be(expectedItemInvocations);
            launchScheduler.StatisticsCounter.FinishTestItemStatisticsCounter.Count.Should().Be(expectedItemInvocations);
            launchScheduler.StatisticsCounter.LogItemStatisticsCounter.Count.Should().BeGreaterOrEqualTo(2 * 5 * 10 / 10); // default logs buffer size for processing is 10
            launchScheduler.StatisticsCounter.ToString().Should().NotBeNullOrEmpty();
        }
    }
}
