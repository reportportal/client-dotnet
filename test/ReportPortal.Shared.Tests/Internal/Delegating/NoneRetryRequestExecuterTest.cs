using FluentAssertions;
using Moq;
using ReportPortal.Shared.Internal.Delegating;
using System;
using System.Net.Http;
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
            executer.Awaiting(e => e.ExecuteAsync(action.Object)).Should().Throw<TaskCanceledException>();

            action.Verify(a => a(), Times.Once());
        }

        [Fact]
        public void ShouldUseThrottler()
        {
            var throttler = new Mock<IRequestExecutionThrottler>();

            var executer = new NoneRetryRequestExecuter(throttler.Object);

            var action = new Mock<Func<Task<string>>>();
            action.Setup(a => a()).Throws<TaskCanceledException>();

            executer.Awaiting(e => e.ExecuteAsync(action.Object)).Should().Throw<Exception>();

            throttler.Verify(t => t.ReserveAsync(), Times.Once());
            throttler.Verify(t => t.Release(), Times.Once());
        }

        [Fact]
        public void ShouldNotInvokeCallbackAction()
        {
            var executer = new NoneRetryRequestExecuter(null);

            var action = new Mock<Func<Task<string>>>();
            action.Setup(a => a()).Throws<TaskCanceledException>();

            var invokedTimes = 0;

            executer.Awaiting(e => e.ExecuteAsync(action.Object, (exp) => invokedTimes++)).Should().Throw<TaskCanceledException>();

            invokedTimes.Should().Be(0);
        }
    }
}
