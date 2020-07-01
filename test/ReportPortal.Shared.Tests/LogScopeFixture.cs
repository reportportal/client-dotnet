using FluentAssertions;
using Moq;
using ReportPortal.Shared.Execution.Logging;
using ReportPortal.Shared.Extensibility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    [Collection("Static")]
    public class LogScopeFixture
    {
        [Fact]
        public void ShouldThrowExceptionIfScopeNameIsNull()
        {
            Action action = () =>
            {
                using (var s = Log.BeginScope(null))
                {

                }
            };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldThrowExceptionIfScopeNameIsEmpty()
        {
            Action action = () =>
            {
                using (var s = Log.BeginScope(""))
                {

                }
            };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldAlwaysHaveActiveScopedLog()
        {
            Log.ActiveScope.Should().BeSameAs(Log.ActiveScope);
            var rootScope = Log.ActiveScope;
            rootScope.Name.Should().BeNull();
            Log.ActiveScope.Should().NotBeNull();
            Log.ActiveScope.Parent.Should().BeNull();

            using (var scope = Log.BeginScope("q"))
            {
                scope.Parent.Should().BeNull();
                scope.Name.Should().Be("q");
                Log.ActiveScope.Should().BeSameAs(scope);

                using (var scope2 = scope.BeginScope("w"))
                {
                    Log.ActiveScope.Should().BeSameAs(scope2);
                    scope2.Parent.Should().Be(scope);
                }

                Log.ActiveScope.Should().BeSameAs(scope);
            }

            Log.ActiveScope.Should().Be(rootScope);
        }

        [Fact]
        public async Task ShouldAlwaysHaveActiveAsyncScopedLog()
        {
            Log.ActiveScope.Should().BeSameAs(Log.ActiveScope);
            Log.ActiveScope.Id.Should().Be(Log.ActiveScope.Id);
            var rootScope = Log.ActiveScope;
            Log.ActiveScope.Should().NotBeNull();

            await Task.Delay(1);

            Log.ActiveScope.Should().BeSameAs(rootScope);

            using (var scope = Log.BeginScope("q"))
            {
                await Task.Delay(1);

                Log.ActiveScope.Should().BeSameAs(scope);
                Log.RootScope.Should().BeSameAs(rootScope);

                using (var scope2 = scope.BeginScope("q"))
                {
                    await Task.Delay(1);

                    Log.ActiveScope.Should().BeSameAs(scope2);

                    var scopeFromOtherContext = await DoSomeWorkAsync(rootScope);
                    scopeFromOtherContext.Should().BeSameAs(scope2);

                    Parallel.For(0, 200, async (from, to) =>
                    {
                        (await DoSomeWorkAsync(rootScope)).Should().BeSameAs(scope2);
                    });


                    var parallelTasks = new List<Task<ILogScope>>();
                    for (int i = 0; i < 200; i++)
                    {
                        parallelTasks.Add(Task.Factory.StartNew(async () => await DoSomeWorkAsync(rootScope)).Unwrap());
                    }
                    Task.WaitAll(parallelTasks.ToArray());
                    parallelTasks.ForEach(t => t.Result.Should().BeSameAs(scope2));

                    var threads = new List<Thread>();
                    for (int i = 0; i < 50; i++)
                    {
                        var thread = new Thread(new ThreadStart(new WorkerState(scope2).Do));
                        thread.Start();
                        threads.Add(thread);
                    }
                    threads.ForEach(th => th.Join());
                }

                Log.ActiveScope.Should().BeSameAs(scope);
            }

            Log.ActiveScope.Should().BeSameAs(rootScope);
        }

        private async Task<ILogScope> DoSomeWorkAsync(ILogScope expectedRootScope)
        {
            await Task.Delay(1);

            using (var scope = Log.BeginScope("q"))
            {
                await Task.Delay(new Random().Next(20));
                Log.RootScope.Should().BeSameAs(expectedRootScope);
            }

            return Log.ActiveScope;
        }

        class WorkerState
        {
            public WorkerState(ILogScope expectedScope)
            {
                ExpectedScope = expectedScope;
            }

            public ILogScope ExpectedScope { get; set; }

            public void Do()
            {
                using (var scope = Log.ActiveScope.BeginScope("q"))
                {
                    Thread.Sleep(new Random().Next(20));
                }

                Log.ActiveScope.Should().Be(ExpectedScope);
            }
        }

        [Fact]
        public void ShouldNotifyLogHandlers()
        {
            var handler = new Mock<ILogHandler>();
            ExtensionManager.Instance.LogHandlers.Add(handler.Object);

            for (int i = 0; i < 5; i++)
            {
                using (var scope = Log.BeginScope("scope"))
                {
                    Log.ActiveScope.Info("abc");
                    Log.RootScope.Info("abc");
                }
            }

            handler.Verify(h => h.Handle(null, It.IsAny<Client.Abstractions.Requests.CreateLogItemRequest>()), Times.Exactly(5));
            handler.Verify(h => h.Handle(It.Is<ILogScope>(ls => ls != null), It.IsAny<Client.Abstractions.Requests.CreateLogItemRequest>()), Times.Exactly(5));
            handler.Verify(h => h.BeginScope(It.IsAny<ILogScope>()), Times.Exactly(5));
            handler.Verify(h => h.EndScope(It.IsAny<ILogScope>()), Times.Exactly(5));

            ExtensionManager.Instance.LogHandlers.Remove(handler.Object);
        }

        [Fact]
        public void ShouldImplicitlySetBeginAndEndTime()
        {
            ILogScope scope = Log.BeginScope("q");

            scope.BeginTime.Should().BeCloseTo(DateTime.UtcNow);
            scope.EndTime.Should().BeNull();

            scope.Dispose();

            scope.EndTime.Should().BeCloseTo(DateTime.UtcNow);
        }

        [Fact]
        public void StatusShouldBeInProgressByDefault()
        {
            using (var scope = Log.BeginScope("a"))
            {
                scope.Status.Should().Be(LogScopeStatus.InProgress);
            }
        }

        [Fact]
        public void StatusOfRootScopeShouldAlwaysBeInProgress()
        {
            Log.RootScope.Status.Should().Be(LogScopeStatus.InProgress);
            Log.RootScope.Status = LogScopeStatus.Passed;
            Log.RootScope.Status.Should().Be(LogScopeStatus.InProgress);
        }

        [Fact]
        public void StatusShouldBeImplicitlyPassedForEndedScope()
        {
            var scope = Log.BeginScope("a");
            scope.Status.Should().Be(LogScopeStatus.InProgress);
            scope.Dispose();
            scope.Status.Should().Be(LogScopeStatus.Passed);
        }

        [Theory]
        [InlineData(LogScopeStatus.Passed)]
        [InlineData(LogScopeStatus.Failed)]
        [InlineData(LogScopeStatus.Skipped)]
        public void ShouldBeAbleToChangeStatus(LogScopeStatus status)
        {
            var scope = Log.BeginScope("a");
            scope.Status = status;
            scope.Status.Should().Be(status);
            scope.Dispose();
            scope.Status.Should().Be(status);
        }
    }
}
