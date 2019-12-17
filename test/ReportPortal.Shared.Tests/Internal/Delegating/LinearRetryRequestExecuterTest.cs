using FluentAssertions;
using Moq;
using ReportPortal.Shared.Internal.Delegating;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests.Internal.Delegating
{
    public class LinearRetryRequestExecuterTest
    {
        [Fact]
        public void DelayShouldBeGreaterOrEqualZero()
        {
            Action ctor = () => new LinearRetryRequestExecuter(int.MaxValue, 1, delay: -1);
            ctor.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void MaxAttemptsShouldBeGreaterOrEqualOne()
        {
            Action ctor = () => new LinearRetryRequestExecuter(int.MaxValue, maxRetryAttempts: 0, 0);
            ctor.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async Task ExecuteValidActionOneTime()
        {
            var action = new Mock<Func<Task<string>>>();

            var executer = new LinearRetryRequestExecuter(int.MaxValue, 3, 2);
            var res = await executer.ExecuteAsync(action.Object);
            res.Should().Be(null);
            action.Verify(a => a(), Times.Once);
        }

        [Fact]
        public void ShouldRetryTaskCanceledExceptionAction()
        {
            var action = new Mock<Func<Task<string>>>();
            action.Setup(a => a()).Throws<TaskCanceledException>();

            var executer = new LinearRetryRequestExecuter(int.MaxValue, 3, 0);
            executer.Awaiting(e => e.ExecuteAsync(action.Object)).Should().Throw<TaskCanceledException>();

            action.Verify(a => a(), Times.Exactly(3));
        }

        [Fact]
        public void ShouldRetryHttpRequestExceptionAction()
        {
            var action = new Mock<Func<Task<string>>>();
            action.Setup(a => a()).Throws<HttpRequestException>();

            var executer = new LinearRetryRequestExecuter(int.MaxValue, 3, 0);
            executer.Awaiting(e => e.ExecuteAsync(action.Object)).Should().Throw<HttpRequestException>();

            action.Verify(a => a(), Times.Exactly(3));
        }

        [Fact]
        public void ShouldNotRetryAnyOtherExceptionAction()
        {
            var action = new Mock<Func<Task<string>>>();
            action.Setup(a => a()).Throws<Exception>();

            var executer = new LinearRetryRequestExecuter(int.MaxValue, 3, 0);
            executer.Awaiting(e => e.ExecuteAsync(action.Object)).Should().Throw<Exception>();

            action.Verify(a => a(), Times.Exactly(1));
        }
    }
}
