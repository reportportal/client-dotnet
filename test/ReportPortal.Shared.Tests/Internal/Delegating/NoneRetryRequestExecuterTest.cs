using FluentAssertions;
using Moq;
using ReportPortal.Shared.Internal.Delegating;
using ReportPortal.Shared.Reporter.Statistics;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests.Internal.Delegating
{
    public class NoneRetryRequestExecuterTest
    {
        [Fact]
        public async Task ExecuteValidActionOneTime()
        {
            var action = new Mock<Func<Task<string>>>();

            var executer = new NoneRetryRequestExecuter(null);
            var res = await executer.ExecuteAsync(action.Object);
            res.Should().Be(null);
            action.Verify(a => a(), Times.Once);
        }

        [Fact]
        public void ShouldNotRetryAnyExceptionAction()
        {
            var action = new Mock<Func<Task<string>>>();
            action.Setup(a => a()).Throws<TaskCanceledException>();

            var executer = new NoneRetryRequestExecuter(null);
            executer.Awaiting(e => e.ExecuteAsync(action.Object)).Should().ThrowAsync<TaskCanceledException>();

            action.Verify(a => a(), Times.Once());
        }

        [Fact]
        public async Task ShouldUseThrottler()
        {
            var throttler = new Mock<IRequestExecutionThrottler>();

            var executer = new NoneRetryRequestExecuter(throttler.Object);

            var action = new Mock<Func<Task<string>>>();
            action.Setup(a => a()).Throws<TaskCanceledException>();

            await executer.Awaiting(e => e.ExecuteAsync(action.Object)).Should().ThrowAsync<Exception>();

            throttler.Verify(t => t.ReserveAsync(), Times.Once());
            throttler.Verify(t => t.Release(), Times.Once());
        }

        [Fact]
        public async Task ShouldNotInvokeCallbackAction()
        {
            var executer = new NoneRetryRequestExecuter(null);

            var action = new Mock<Func<Task<string>>>();
            action.Setup(a => a()).Throws<TaskCanceledException>();

            var invokedTimes = 0;

            await executer.Awaiting(e => e.ExecuteAsync(action.Object, (exp) => invokedTimes++)).Should().ThrowAsync<TaskCanceledException>();

            invokedTimes.Should().Be(0);
        }

        [Fact]
        public async Task ShouldMeasureRequestsStatistics()
        {
            var counter = new Mock<IStatisticsCounter>();

            var executer = new NoneRetryRequestExecuter(null);

            var action = new Mock<Func<Task<string>>>();
            action.Setup(a => a()).ReturnsAsync("a");

            await executer.ExecuteAsync(action.Object, null, counter.Object);

            counter.Verify(c => c.Measure(It.IsAny<TimeSpan>()), Times.Once);
        }
    }
}
