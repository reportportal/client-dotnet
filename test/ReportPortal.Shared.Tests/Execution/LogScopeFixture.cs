using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Shared.Execution;
using ReportPortal.Shared.Execution.Logging;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Commands;
using ReportPortal.Shared.Extensibility.Commands.CommandArgs;
using System;
using System.Collections.Generic;
using Xunit;

namespace ReportPortal.Shared.Tests.Execution
{
    public class LogScopeFixture
    {
        [Fact]
        public void ShouldRaiseTestBeginNewScope()
        {
            ILogContext tc = null;
            ILogScope logScope = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnBeginLogScopeCommand += (a, b) => { tc = a; logScope = b; };
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var testContext = new TestContext(extensionManager, new CommandsSource(new List<ICommandsListener> { listener.Object }));

            testContext.Log.BeginScope("qwe");

            tc.Should().Be(testContext);
            logScope.Name.Should().Be("qwe");
            logScope.Context.Should().Be(testContext);
        }

        [Fact]
        public void ShouldRaiseTestEndScope()
        {
            ILogContext tc = null;
            ILogScope logScope = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnEndLogScopeCommand += (a, b) => { tc = a; logScope = b; };
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var testContext = new TestContext(extensionManager, new CommandsSource(new List<ICommandsListener> { listener.Object }));

            var scope = testContext.Log.BeginScope("qwe");
            scope.Dispose();

            tc.Should().Be(testContext);
            logScope.Name.Should().Be("qwe");
            logScope.EndTime.Should().BeCloseTo(DateTime.UtcNow);
        }

        [Fact]
        public void ShouldRaiseTestLogMessageForRootedScope()
        {
            ILogContext tc = null;
            LogMessageCommandArgs logMessage = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnLogMessageCommand += (a, b) => { tc = a; logMessage = b; };
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var testContext = new TestContext(extensionManager, new CommandsSource(new List<ICommandsListener> { listener.Object }));

            testContext.Log.Info("qwe");

            tc.Should().Be(testContext);
            logMessage.LogItemRequest.Text.Should().Be("qwe");
            logMessage.LogItemRequest.Level.Should().Be(LogLevel.Info);
            logMessage.LogScope.Should().BeNull();
        }

        [Fact]
        public void ShouldRaiseTestLogMessageForInnerScope()
        {
            ILogContext tc = null;
            LogMessageCommandArgs logMessage = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnLogMessageCommand += (a, b) => { tc = a; logMessage = b; };
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var testContext = new TestContext(extensionManager, new CommandsSource(new List<ICommandsListener> { listener.Object }));

            var scope = testContext.Log.BeginScope("qwe");
            testContext.Log.Info("asd");

            tc.Should().Be(testContext);
            logMessage.LogItemRequest.Text.Should().Be("asd");
            logMessage.LogScope.Should().Be(testContext.Log);
        }

        [Fact]
        public void ShouldRaiseLaunchBeginNewScope()
        {
            ILogContext lc = null;
            ILogScope logScope = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnBeginLogScopeCommand += (a, b) => { lc = a; logScope = b; };
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var launchContext = new LaunchContext(extensionManager, new CommandsSource(new List<ICommandsListener> { listener.Object }));

            launchContext.Log.BeginScope("qwe");

            lc.Should().Be(launchContext);
            logScope.Name.Should().Be("qwe");
            logScope.Context.Should().Be(launchContext);
        }

        [Fact]
        public void ShouldRaiseLaunchEndScope()
        {
            ILogContext lc = null;
            ILogScope logScope = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnEndLogScopeCommand += (a, b) => { lc = a; logScope = b; };
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var launchContext = new LaunchContext(extensionManager, new CommandsSource(new List<ICommandsListener> { listener.Object }));

            var scope = launchContext.Log.BeginScope("qwe");
            scope.Dispose();

            lc.Should().Be(launchContext);
            logScope.Name.Should().Be("qwe");
            logScope.EndTime.Should().BeCloseTo(DateTime.UtcNow);
        }

        [Fact]
        public void ShouldRaiseLaunchLogMessageForRootedScope()
        {
            ILogContext lc = null;
            LogMessageCommandArgs logMessage = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnLogMessageCommand += (a, b) => { lc = a; logMessage = b; };
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var launchContext = new LaunchContext(extensionManager, new CommandsSource(new List<ICommandsListener> { listener.Object }));

            launchContext.Log.Info("qwe");

            lc.Should().Be(launchContext);
            logMessage.LogItemRequest.Text.Should().Be("qwe");
            logMessage.LogItemRequest.Level.Should().Be(LogLevel.Info);
            logMessage.LogScope.Should().BeNull();
        }

        [Fact]
        public void ShouldRaiseLaunchLogMessageForInnerScope()
        {
            ILogContext lc = null;
            LogMessageCommandArgs logMessage = null;

            var listener = new Mock<ICommandsListener>();
            listener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.OnLogMessageCommand += (a, b) => { lc = a; logMessage = b; };
            });

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener.Object);

            var launchContext = new LaunchContext(extensionManager, new CommandsSource(new List<ICommandsListener> { listener.Object }));

            var scope = launchContext.Log.BeginScope("qwe");
            launchContext.Log.Info("asd");

            lc.Should().Be(launchContext);
            logMessage.LogItemRequest.Text.Should().Be("asd");
            logMessage.LogScope.Should().Be(launchContext.Log);
        }
    }
}
