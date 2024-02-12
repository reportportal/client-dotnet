using FluentAssertions;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Internal.Delegating;
using System;
using System.Net;
using Xunit;

namespace ReportPortal.Shared.Tests.Internal.Delegating
{
    public class RequestExecuterFactoryTest
    {
        [Fact]
        public void ShouldThrowExceptionForNullConfiguration()
        {
            Action ctor = () => new RequestExecuterFactory(null);
            ctor.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldCreateDefaultExponentialExecuter()
        {
            var configuration = new ConfigurationBuilder().Build();
            var factory = new RequestExecuterFactory(configuration);
            var executer = factory.Create();

            executer.Should().BeOfType<ExponentialRetryRequestExecuter>();
            var exponentialExecuter = executer as ExponentialRetryRequestExecuter;
            exponentialExecuter.MaxRetryAttemps.Should().Be(3);
            exponentialExecuter.BaseIndex.Should().Be(2);
        }

        [Fact]
        public void ShouldCreateLinearExecuter()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["Server:Retry:Strategy"] = "linear";
            var factory = new RequestExecuterFactory(configuration);
            var executer = factory.Create();

            executer.Should().BeOfType<LinearRetryRequestExecuter>();
            var linearExecuter = executer as LinearRetryRequestExecuter;
            linearExecuter.MaxRetryAttemps.Should().Be(3);
            linearExecuter.Delay.Should().Be(5000);
            linearExecuter.HttpStatusCodes.Should().BeNull();
        }

        [Fact]
        public void ShouldThrowExceptionForUnknownStrategy()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["Server:Retry:Strategy"] = "any_unknown_value";
            var factory = new RequestExecuterFactory(configuration);

            factory.Invoking((f) => f.Create()).Should().Throw<Exception>().WithMessage("*any_unknown_value*");
        }

        [Fact]
        public void ConfigurableExponentialRetry()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["Server:Retry:Strategy"] = "exponential";
            configuration.Properties["Server:Retry:MaxAttempts"] = 5;
            configuration.Properties["Server:Retry:BaseIndex"] = 6;
            configuration.Properties["Server:Retry:HttpStatusCodes"] = "500";
            var executer = new RequestExecuterFactory(configuration).Create() as ExponentialRetryRequestExecuter;

            executer.MaxRetryAttemps.Should().Be(5);
            executer.BaseIndex.Should().Be(6);
            executer.HttpStatusCodes.Should().BeEquivalentTo(new HttpStatusCode[] { HttpStatusCode.InternalServerError });
        }

        [Fact]
        public void ConfigurableLinearRetry()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["Server:Retry:Strategy"] = "linear";
            configuration.Properties["Server:Retry:MaxAttempts"] = 5;
            configuration.Properties["Server:Retry:Delay"] = 6000;
            configuration.Properties["Server:Retry:HttpStatusCodes"] = "500";
            var executer = new RequestExecuterFactory(configuration).Create() as LinearRetryRequestExecuter;

            executer.MaxRetryAttemps.Should().Be(5);
            executer.Delay.Should().Be(6000);
            executer.HttpStatusCodes.Should().BeEquivalentTo(new HttpStatusCode[] { HttpStatusCode.InternalServerError});
        }

        [Fact]
        public void ShouldCreateNoneExecuter()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["Server:Retry:Strategy"] = "none";
            var executer = new RequestExecuterFactory(configuration).Create();
            executer.Should().BeOfType<NoneRetryRequestExecuter>();
        }
    }
}
