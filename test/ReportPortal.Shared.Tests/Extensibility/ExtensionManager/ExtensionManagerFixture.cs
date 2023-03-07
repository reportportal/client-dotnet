using FluentAssertions;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Commands;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.ExtensionManager
{
    public class ExtensionManagerFixture
    {
        [Fact]
        public void ShouldExploreExtensions()
        {
            var manager = new Shared.Extensibility.ExtensionManager();
            manager.Explore(Environment.CurrentDirectory);

            manager.ReportEventObservers.Count.Should()
                .Be(2, "default and google analytic observers should be registered by default");

            manager.CommandsListeners.Should().HaveCount(1);
        }

        [Fact]
        public void ShouldSkipIncorrectExtensions()
        {
            var manager = new Shared.Extensibility.ExtensionManager();
            manager.Explore("Extensibility/ExtensionManager/Data");
        }
    }

    public class MyCommandListenerExtension : ICommandsListener
    {
        public void Initialize(ICommandsSource commandsSource)
        {

        }
    }
}
