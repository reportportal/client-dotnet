using FluentAssertions;
using ReportPortal.Shared.Execution.Logging;
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
        public void LaunchContextShouldBeTheSame()
        {
            Context.Launch.Should().NotBeNull();
            Context.Launch.Should().BeSameAs(Context.Launch);
        }

        [Fact]
        public void ShouldThrowExceptionIfScopeNameIsNull()
        {
            Action action = () =>
            {
                using (var s = Context.Current.Log.BeginScope(null))
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
                using (var s = Context.Current.Log.BeginScope(""))
                {

                }
            };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldAlwaysHaveActiveScopedLog()
        {
            Context.Current.Log.Should().BeSameAs(Context.Current.Log);
            var rootScope = Context.Current.Log;
            rootScope.Name.Should().BeNull();
            Context.Current.Log.Should().NotBeNull();
            Context.Current.Log.Parent.Should().BeNull();

            using (var scope = Context.Current.Log.BeginScope("q"))
            {
                scope.Parent.Should().BeNull();
                scope.Name.Should().Be("q");
                Context.Current.Log.Should().BeSameAs(scope);

                using (var scope2 = scope.BeginScope("w"))
                {
                    Context.Current.Log.Should().BeSameAs(scope2);
                    scope2.Parent.Should().Be(scope);
                }

                Context.Current.Log.Should().BeSameAs(scope);
            }

            Context.Current.Log.Should().Be(rootScope);
        }

        [Fact]
        public async Task ShouldAlwaysHaveActiveAsyncScopedLog()
        {
            Context.Current.Log.Should().BeSameAs(Context.Current.Log);
            Context.Current.Log.Parent.Should().BeNull();
            Context.Current.Log.Id.Should().Be(Context.Current.Log.Id);
            var rootScope = Context.Current.Log;
            Context.Current.Log.Should().NotBeNull();

            await Task.Delay(1);

            Context.Current.Log.Should().BeSameAs(rootScope);

            using (var scope = Context.Current.Log.BeginScope("q"))
            {
                await Task.Delay(1);

                Context.Current.Log.Should().BeSameAs(scope);
                Context.Current.Log.Root.Should().BeSameAs(rootScope);

                using (var scope2 = scope.BeginScope("q"))
                {
                    await Task.Delay(1);

                    Context.Current.Log.Should().BeSameAs(scope2);

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

                Context.Current.Log.Should().BeSameAs(scope);
            }

            Context.Current.Log.Should().BeSameAs(rootScope);
        }

        private async Task<ILogScope> DoSomeWorkAsync(ILogScope expectedRootScope)
        {
            await Task.Delay(1);

            using (var scope = Context.Current.Log.BeginScope("q"))
            {
                await Task.Delay(new Random().Next(20));
                Context.Current.Log.Root.Should().BeSameAs(expectedRootScope);
            }

            return Context.Current.Log;
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
                using (var scope = Context.Current.Log.BeginScope("q"))
                {
                    Thread.Sleep(new Random().Next(20));
                }

                Context.Current.Log.Should().Be(ExpectedScope);
            }
        }

        [Fact]
        public void ShouldImplicitlySetBeginAndEndTime()
        {
            ILogScope scope = Context.Current.Log.BeginScope("q");

            scope.BeginTime.Should().BeCloseTo(DateTime.UtcNow);
            scope.EndTime.Should().BeNull();

            scope.Dispose();

            scope.EndTime.Should().BeCloseTo(DateTime.UtcNow);
        }

        [Fact]
        public void StatusShouldBeInProgressByDefault()
        {
            using (var scope = Context.Current.Log.BeginScope("a"))
            {
                scope.Status.Should().Be(LogScopeStatus.InProgress);
            }
        }

        [Fact]
        public void StatusOfRootScopeShouldAlwaysBeInProgress()
        {
            Context.Current.Log.Status.Should().Be(LogScopeStatus.InProgress);
            Context.Current.Log.Status = LogScopeStatus.Passed;
            Context.Current.Log.Status.Should().Be(LogScopeStatus.InProgress);
        }

        [Fact]
        public void StatusShouldBeImplicitlyPassedForEndedScope()
        {
            var scope = Context.Current.Log.BeginScope("a");
            scope.Status.Should().Be(LogScopeStatus.InProgress);
            scope.Dispose();
            scope.Status.Should().Be(LogScopeStatus.Passed);
        }

        [Theory]
        [InlineData(LogScopeStatus.Passed)]
        [InlineData(LogScopeStatus.Failed)]
        [InlineData(LogScopeStatus.Skipped)]
        [InlineData(LogScopeStatus.Warn)]
        [InlineData(LogScopeStatus.Info)]
        public void ShouldBeAbleToChangeStatus(LogScopeStatus status)
        {
            var scope = Context.Current.Log.BeginScope("a");
            scope.Status = status;
            scope.Status.Should().Be(status);
            scope.Dispose();
            scope.Status.Should().Be(status);
        }
    }
}
