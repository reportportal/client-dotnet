using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Shared.Execution;
using ReportPortal.Shared.Execution.Logging;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Commands;
using ReportPortal.Shared.Extensibility.Commands.CommandArgs;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Execution
{
    public class LogScopeFixture
    {
        [Fact]
        public void ShouldRaiseBeginNewScope()
        {
            ILogScope logScope = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnBeginLogScopeCommand += (a, b) => logScope = b;
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var testContext = new TestContext(extensionManager, Mock.Of<CommandsSource>());

            testContext.Log.BeginScope("qwe");

            logScope.Name.Should().Be("qwe");
        }

        [Fact]
        public void ShouldRaiseEndScope()
        {
            ILogScope logScope = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnEndLogScopeCommand += (a, b) => logScope = b;
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var testContext = new TestContext(extensionManager, Mock.Of<CommandsSource>());

            var scope = testContext.Log.BeginScope("qwe");
            scope.Dispose();

            logScope.Name.Should().Be("qwe");
            logScope.EndTime.Should().BeCloseTo(DateTime.UtcNow);
        }

        [Fact]
        public void ShouldRaiseLogMessageForRootedScope()
        {
            LogMessageCommandArgs logMessage = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnLogMessageCommand += (a, b) => logMessage = b;
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var testContext = new TestContext(extensionManager, Mock.Of<CommandsSource>());

            testContext.Log.Info("qwe");

            logMessage.LogItemRequest.Text.Should().Be("qwe");
            logMessage.LogItemRequest.Level.Should().Be(LogLevel.Info);
            logMessage.LogScope.Should().BeNull();
        }

        [Fact]
        public void ShouldRaiseLogMessageForInnerScope()
        {
            LogMessageCommandArgs logMessage = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnLogMessageCommand += (a, b) => logMessage = b;
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var testContext = new TestContext(extensionManager, Mock.Of<CommandsSource>());

            var scope = testContext.Log.BeginScope("qwe");
            testContext.Log.Info("asd");

            logMessage.LogItemRequest.Text.Should().Be("asd");
            logMessage.LogScope.Should().Be(testContext.Log);
        }
    }
}
