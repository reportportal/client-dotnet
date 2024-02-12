using FluentAssertions;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Internal.Delegating;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Internal.Delegating
{
    public class RequestExecutionThrottleFactoryTest
    {
        [Fact]
        public void ShouldThrowExceptionForNullConfiguration()
        {
            Action ctor = () => new RequestExecutionThrottleFactory(null);
            ctor.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldThrowExceptionForIncorrectMaxRequests()
        {
            Action ctor = () => new RequestExecutionThrottler(0);
            ctor.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldCreateAndDisposeDefaultThrottler()
        {
            var configuration = new ConfigurationBuilder().Build();
            var factory = new RequestExecutionThrottleFactory(configuration);
            var throttler = factory.Create();

            throttler.Should().BeOfType<RequestExecutionThrottler>();

            (throttler as RequestExecutionThrottler).Dispose();
        }

        [Fact]
        public void ShouldCreateThrottler()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["Server:MaximumConnectionsNumber"] = "5";
            var factory = new RequestExecutionThrottleFactory(configuration);
            var ithrottler = factory.Create();

            ithrottler.Should().BeOfType<RequestExecutionThrottler>();
            var throttler = ithrottler as RequestExecutionThrottler;
            throttler.MaxCapacity.Should().Be(5);
        }
    }
}
