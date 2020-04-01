using FluentAssertions;
using Moq;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    [CollectionDefinition(nameof(LogScopeFixture), DisableParallelization = true)]
    public class LogScopeFixture
    {
        [Fact]
        public void ShouldThrowExceptionIfScopeNameIsNull()
        {
            Action action = () =>
            {
                using (var s = Log.BeginNewScope(null))
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
                using (var s = Log.BeginNewScope(""))
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
            Log.ActiveScope.Should().NotBeNull();

            using (var scope = Log.BeginNewScope("q"))
            {
                Log.ActiveScope.Should().BeSameAs(scope);

                using (var scope2 = scope.BeginNewScope("q"))
                {
                    Log.ActiveScope.Should().BeSameAs(scope2);
                }

                Log.ActiveScope.Should().BeSameAs(scope);
            }

            Log.ActiveScope.Should().BeSameAs(rootScope);
        }

        [Fact]
        public async Task ShouldAlwaysHaveActiveAsyncScopedLog()
        {
            Log.ActiveScope.Should().BeSameAs(Log.ActiveScope);
            var rootScope = Log.ActiveScope;
            Log.ActiveScope.Should().NotBeNull();

            await Task.Delay(1);

            Log.ActiveScope.Should().BeSameAs(rootScope);

            using (var scope = Log.BeginNewScope("q"))
            {
                await Task.Delay(1);

                Log.ActiveScope.Should().BeSameAs(scope);
                Log.RootScope.Should().BeSameAs(rootScope);

                using (var scope2 = scope.BeginNewScope("q"))
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

            using (var scope = Log.BeginNewScope("q"))
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
                using (var scope = Log.ActiveScope.BeginNewScope("q"))
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
            Bridge.LogHandlerExtensions.Add(handler.Object);

            Log.Info("abc");
            for (int i = 0; i < 5; i++)
            {
                using (var scope = Log.BeginNewScope("scope"))
                {
                    Log.Info("qwe");

                    Log.RootScope.Info("abc");
                }
            }
            Log.Info("abc");

            handler.Verify(h => h.Handle(null, It.IsAny<Client.Abstractions.Requests.CreateLogItemRequest>()), Times.Exactly(7));
            handler.Verify(h => h.Handle(It.Is<ILogScope>(ls => ls != null), It.IsAny<Client.Abstractions.Requests.CreateLogItemRequest>()), Times.Exactly(5));
            handler.Verify(h => h.BeginScope(It.IsAny<ILogScope>()), Times.Exactly(5));
            handler.Verify(h => h.EndScope(It.IsAny<ILogScope>()), Times.Exactly(5));

            Bridge.LogHandlerExtensions.Remove(handler.Object);
        }
    }
}
