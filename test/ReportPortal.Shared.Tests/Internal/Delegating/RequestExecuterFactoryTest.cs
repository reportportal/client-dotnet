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

        [Fact]
        public void ShouldCreateLinearExecuter()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c.GetValue("Server:Retry:Strategy", It.IsAny<string>())).Returns("linear");
            var factory = new RequestExecuterFactory();
            var executer = factory.Create(configuration.Object);

            executer.Should().BeOfType<LinearRetryRequestExecuter>();
            var exponentialExecuter = executer as LinearRetryRequestExecuter;
            exponentialExecuter.MaxRetryAttemps.Should().Be(3);
            exponentialExecuter.Delay.Should().Be(5000);
        }

        [Fact]
        public void ShouldThrowExceptionForUnknownStrategy()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c.GetValue("Server:Retry:Strategy", It.IsAny<string>())).Returns("any_unknown_value");
            var factory = new RequestExecuterFactory();

            factory.Invoking((f) => f.Create(configuration.Object)).Should().Throw<ArgumentOutOfRangeException>().WithMessage("*any_unknown_value*");
        }
    }
}
