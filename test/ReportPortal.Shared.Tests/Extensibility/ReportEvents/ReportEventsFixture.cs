using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Tests.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.ReportEvents
{
    public class ReportEventsFixture
    {
        [Fact]
        public void ShouldNotBreakReportingIfInitializationThrowsException()
        {
            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Throws<Exception>();

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0);
            Action act = () => launch.Sync();
            act.Should().NotThrow();
        }

        [Fact]
        public void ShouldNotBreakReportingIfHandlerThrowsException()
        {
            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnLaunchInitializing += (a, b) => throw new Exception();

                s.OnBeforeLaunchStarting += (a, b) => throw new Exception();
                s.OnAfterLaunchStarted += (a, b) => throw new Exception();
                s.OnBeforeLaunchFinishing += (a, b) => throw new Exception();
                s.OnAfterLaunchFinished += (a, b) => throw new Exception();

                s.OnBeforeTestStarting += (a, b) => throw new Exception();
                s.OnAfterTestStarted += (a, b) => throw new Exception();
                s.OnBeforeTestFinishing += (a, b) => throw new Exception();
                s.OnAfterTestFinished += (a, b) => throw new Exception();
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0);
            Action act = () => launch.Sync();
            act.Should().NotThrow();
        }

        [Fact]
        public void ShouldNotBreakReportingIfLogsHandlerThrowsException()
        {
            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnBeforeLogsSending += (a, b) => throw new Exception();
                s.OnAfterLogsSent += (a, b) => throw new Exception();
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 1, 1);
            Action act = () => launch.Sync();
            act.Should().NotThrow();
        }

        [Fact]
        public void ShouldNotifyBeforeItemStarting()
        {
            ILaunchReporter l = null;
            ITestReporter t = null;
            BeforeLaunchStartingEventArgs beforeLaunchStartingEventArgs = null;
            BeforeTestStartingEventArgs beforeTestStartingEventArgs = null;

            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnBeforeLaunchStarting += (a, b) => { l = a; beforeLaunchStartingEventArgs = b; };
                s.OnBeforeTestStarting += (a, b) => { t = a; beforeTestStartingEventArgs = b; };
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0);
            launch.Sync();

            beforeLaunchStartingEventArgs.ClientService.Should().BeSameAs(client);
            l.Should().BeSameAs(launch);
            beforeLaunchStartingEventArgs.StartLaunchRequest.Should().NotBeNull();

            beforeTestStartingEventArgs.ClientService.Should().BeSameAs(client);
            t.Should().BeSameAs(launch.ChildTestReporters[0]);
            beforeTestStartingEventArgs.StartTestItemRequest.Should().NotBeNull();
        }

        [Fact]
        public void ShouldNotifyAfterItemStarted()
        {
            ILaunchReporter l = null;
            ITestReporter t = null;
            AfterLaunchStartedEventArgs afterLaunchStartedEventArgs = null;
            AfterTestStartedEventArgs afterTestStartedEventArgs = null;

            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnAfterLaunchStarted += (a, b) => { l = a; afterLaunchStartedEventArgs = b; };
                s.OnAfterTestStarted += (a, b) => { t = a; afterTestStartedEventArgs = b; };
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0);
            launch.Sync();

            afterLaunchStartedEventArgs.ClientService.Should().BeSameAs(client);
            l.Should().BeSameAs(launch);

            afterTestStartedEventArgs.ClientService.Should().BeSameAs(client);
            t.Should().BeSameAs(launch.ChildTestReporters[0]);
        }

        [Fact]
        public void ShouldNotifyBeforeItemFinishing()
        {
            ILaunchReporter l = null;
            ITestReporter t = null;
            BeforeLaunchFinishingEventArgs beforeLaunchFinishingEventArgs = null;
            BeforeTestFinishingEventArgs beforeTestFinishingEventArgs = null;

            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnBeforeLaunchFinishing += (a, b) => { l = a; beforeLaunchFinishingEventArgs = b; };
                s.OnBeforeTestFinishing += (a, b) => { t = a; beforeTestFinishingEventArgs = b; };
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0);
            launch.Sync();

            beforeLaunchFinishingEventArgs.ClientService.Should().BeSameAs(client);
            l.Should().BeSameAs(launch);
            beforeLaunchFinishingEventArgs.FinishLaunchRequest.Should().NotBeNull();

            beforeTestFinishingEventArgs.ClientService.Should().BeSameAs(client);
            t.Should().BeSameAs(launch.ChildTestReporters[0]);
            beforeTestFinishingEventArgs.FinishTestItemRequest.Should().NotBeNull();
        }

        [Fact]
        public void ShouldNotifyOnLaunchInitializing()
        {
            ILaunchReporter l = null;

            LaunchInitializingEventArgs launchInitializingEventArgs = null;

            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnLaunchInitializing += (a, b) => { l = a; launchInitializingEventArgs = b; };
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0);

            launchInitializingEventArgs.ClientService.Should().BeSameAs(client);
            launchInitializingEventArgs.Configuration.Should().NotBeNull();
            l.Should().BeSameAs(launch);
        }

        [Fact]
        public void ShouldNotifyAfterItemFinished()
        {
            ILaunchReporter l = null;
            ITestReporter t = null;
            AfterLaunchFinishedEventArgs afterLaunchFinishedEventArgs = null;
            AfterTestFinishedEventArgs afterTestFinishedEventArgs = null;

            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnAfterLaunchFinished += (a, b) => { l = a; afterLaunchFinishedEventArgs = b; };
                s.OnAfterTestFinished += (a, b) => { t = a; afterTestFinishedEventArgs = b; };
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0);
            launch.Sync();

            afterLaunchFinishedEventArgs.ClientService.Should().BeSameAs(client);
            l.Should().BeSameAs(launch);

            afterTestFinishedEventArgs.ClientService.Should().BeSameAs(client);
            t.Should().BeSameAs(launch.ChildTestReporters[0]);
        }

        [Fact]
        public void ShouldBeAbleToChangeRequestBeforeLaunchStarting()
        {
            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnBeforeLaunchStarting += (a, b) => b.StartLaunchRequest.Name = "NewName";
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0);
            launch.Sync();

            launch.Info.Name.Should().Be("NewName");
        }

        [Fact]
        public void ShouldBeAbleToChangeRequestBeforeTestStarting()
        {
            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnBeforeTestStarting += (a, b) => b.StartTestItemRequest.Name = "NewName";
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 0, 0);
            launch.Sync();

            launch.ChildTestReporters[0].Info.Name.Should().Be("NewName");
        }

        [Fact]
        public void ShouldNotifyBeforeLogsSending()
        {
            IList<CreateLogItemRequest> logRequests = null;

            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnBeforeLogsSending += (a, b) => logRequests = b.CreateLogItemRequests;
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 1, 1);
            launch.Sync();

            logRequests.Should().HaveCount(1);
        }

        [Fact]
        public void ShouldBeAbleToModifyRequestsBeforeLogsSending()
        {
            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnBeforeLogsSending += (a, b) => b.CreateLogItemRequests.RemoveAt(0);
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            CreateLogItemRequest[] sentClientLogs = null;

            var clientMock = new MockServiceBuilder().Build();
            clientMock.Setup(c => c.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>())).Callback<CreateLogItemRequest[]>(lgs => sentClientLogs = lgs);
            var client = clientMock.Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 1, 3);
            launch.Sync();

            sentClientLogs.Should().HaveCount(2);
        }

        [Fact]
        public void ShouldNotifyAfterLogsSent()
        {
            IReadOnlyList<CreateLogItemRequest> logRequests = null;

            var observer = new Mock<IReportEventsObserver>();
            observer.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnAfterLogsSent += (a, b) => logRequests = b.CreateLogItemRequests;
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(observer.Object);

            var client = new MockServiceBuilder().Build().Object;
            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 1, 1);
            launch.Sync();

            logRequests.Should().HaveCount(1);
        }

        [Fact]
        public void ShouldNotBreakNotificationIfOneHandlerThrowsException()
        {
            var badObserver = new Mock<IReportEventsObserver>();
            badObserver.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnLaunchInitializing += (l, args) => { throw new Exception(); };

                s.OnBeforeLaunchStarting += (l, args) => { throw new Exception(); };
                s.OnAfterLaunchStarted += (l, args) => { throw new Exception(); };

                s.OnBeforeLaunchFinishing += (l, args) => { throw new Exception(); };
                s.OnAfterLaunchFinished += (l, args) => { throw new Exception(); };

                s.OnBeforeTestStarting += (l, args) => { throw new Exception(); };
                s.OnAfterTestStarted += (l, args) => { throw new Exception(); };

                s.OnBeforeTestFinishing += (l, args) => { throw new Exception(); };
                s.OnAfterTestFinished += (l, args) => { throw new Exception(); };

                s.OnBeforeLogsSending += (l, args) => { throw new Exception(); };
            });

            var goodObserver = new Mock<IReportEventsObserver>();

            var onLaunchInitializing = false;

            var onBeforeLaunchStarting = false;
            var onAfterLaunchStarted = false;

            var onBeforeLaunchFinishing = false;
            var onAfterLaunchFinished = false;

            var onBeforeTestStarting = false;
            var onAfterTestStarted = false;

            var onBeforeTestFinishing = false;
            var onAfterTestFinished = false;

            var onBeforeLogsSending = false;
            var onAfterLogsSent = false;

            goodObserver.Setup(o => o.Initialize(It.IsAny<IReportEventsSource>())).Callback<IReportEventsSource>(s =>
            {
                s.OnLaunchInitializing += (l, args) => { onLaunchInitializing = true; };

                s.OnBeforeLaunchStarting += (l, args) => { onBeforeLaunchStarting = true; };
                s.OnAfterLaunchStarted += (l, args) => { onAfterLaunchStarted = true; };

                s.OnBeforeLaunchFinishing += (l, args) => { onBeforeLaunchFinishing = true; };
                s.OnAfterLaunchFinished += (l, args) => { onAfterLaunchFinished = true; };

                s.OnBeforeTestStarting += (l, args) => { onBeforeTestStarting = true; };
                s.OnAfterTestStarted += (l, args) => { onAfterTestStarted = true; };

                s.OnBeforeTestFinishing += (l, args) => { onBeforeTestFinishing = true; };
                s.OnAfterTestFinished += (l, args) => { onAfterTestFinished = true; };

                s.OnBeforeLogsSending += (l, args) => { onBeforeLogsSending = true; };
                s.OnAfterLogsSent += (l, args) => { onAfterLogsSent = true; };
            });

            var extManager = new Shared.Extensibility.ExtensionManager();
            extManager.ReportEventObservers.Add(badObserver.Object);
            extManager.ReportEventObservers.Add(goodObserver.Object);

            var clientMock = new MockServiceBuilder().Build();
            var client = clientMock.Object;

            var launch = new LaunchReporterBuilder(client).With(extManager).Build(1, 1, 1);
            launch.Sync();

            onLaunchInitializing.Should().BeTrue();

            onBeforeLaunchStarting.Should().BeTrue();
            onAfterLaunchStarted.Should().BeTrue();

            onBeforeLaunchFinishing.Should().BeTrue();
            onAfterLaunchFinished.Should().BeTrue();

            onBeforeTestStarting.Should().BeTrue();
            onAfterTestStarted.Should().BeTrue();

            onBeforeTestFinishing.Should().BeTrue();
            onAfterTestFinished.Should().BeTrue();

            onBeforeLogsSending.Should().BeTrue();
            onAfterLogsSent.Should().BeTrue();
        }
    }
}
