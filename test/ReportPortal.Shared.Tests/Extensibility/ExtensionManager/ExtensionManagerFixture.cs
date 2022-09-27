using FluentAssertions;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.ExtensionManager
{
    public class ExtensionManagerFixture
    {
        [Fact]
        public void ShouldHaveEmptyExtensions()
        {
            var manager = new Shared.Extensibility.ExtensionManager();
            manager.LogFormatters.Count.Should().Be(0);
        }

        [Fact]
        public void ShouldExploreExtensions()
        {
            var manager = new Shared.Extensibility.ExtensionManager();
            manager.Explore(Environment.CurrentDirectory);

            manager.LogFormatters.Count.Should().Be(0, "there is no embedded LogFormatters");
            manager.ReportEventObservers.Count.Should().Be(1, "google analytic event observer");
        }
    }
}
