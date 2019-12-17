using FluentAssertions;
using Moq;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Internal.Delegating;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Internal.Delegating
{
    public class RequestExecuterFactoryTest
    {
        [Fact]
        public void ShouldThrowExceptionForNullConfiguration()
        {
            var factory = new RequestExecuterFactory();
            factory.Invoking((f) => f.Create(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldCreateDefaultExponentialExecuter()
        {
            var configuration = new Mock<IConfiguration>();
            var factory = new RequestExecuterFactory();
            var executer = factory.Create(configuration.Object);

            executer.Should().BeOfType<ExponentialRetryRequestExecuter>();
            var exponentialExecuter = executer as ExponentialRetryRequestExecuter;
            exponentialExecuter.MaxRetryAttemps.Should().Be(3);
            exponentialExecuter.BaseIndex.Should().Be(2);
        }
    }
}
