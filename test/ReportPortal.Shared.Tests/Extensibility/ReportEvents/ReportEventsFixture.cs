using FluentAssertions;
using Moq;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Tests.Helpers;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.ReportEvents
{
    public class ReportEventsFixture
    {
        [Fact]
        public void ShouldNotBreakReportingIfInitializtionThrowsException()
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

            launch.LaunchInfo.Name.Should().Be("NewName");
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

            launch.ChildTestReporters[0].TestInfo.Name.Should().Be("NewName");
        }

    }
}
