using FluentAssertions;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Reporter.Http;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Reporter.Http
{
    public class HttpClientFactoryTest
    {
        private readonly System.Net.Http.HttpClientHandler _handler = new System.Net.Http.HttpClientHandler();

        [Fact]
        public void ShouldCreateClient()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["server:url"] = "http://abc.com";
            configuration.Properties["server:authentication:uuid"] = "123";

            var factory = new HttpClientFactory(configuration, _handler);

            var httpClient = factory.Create();
            httpClient.Should().NotBeNull();

            httpClient.BaseAddress.Should().Be("http://abc.com/api/v1/");
            httpClient.DefaultRequestHeaders.Authorization.Should().NotBeNull();
            httpClient.DefaultRequestHeaders.Authorization.Scheme.Should().Be("Bearer");
            httpClient.DefaultRequestHeaders.Authorization.Parameter.Should().Be("123");
        }

        [Fact]
        public void ShouldThrowExceptionWhenServerUrlIsMissing()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["server:authentication:uuid"] = "123";

            var factory = new HttpClientFactory(configuration, _handler);

            Action act = () => factory.Create();

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenServerAuthenticationUuidIsMissing()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["server:url"] = "http://abc.com";

            var factory = new HttpClientFactory(configuration, _handler);

            Action act = () => factory.Create();

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void ConfigurationShouldBeMandatory()
        {
            IConfiguration configuration = null;

            Action ctor1 = () => new HttpClientFactory(configuration, _handler);

            ctor1.Should().ThrowExactly<ArgumentNullException>().And.Message.Should().Contain("configuration");

            Action ctor2 = () => new HttpClientFactory(new ConfigurationBuilder().Build(), null);

            ctor2.Should().ThrowExactly<ArgumentNullException>().And.Message.Should().Contain("httpClientHandler");
        }
    }
}
