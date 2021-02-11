using FluentAssertions;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Reporter.Http;
using System;
using System.Net;
using Xunit;

namespace ReportPortal.Shared.Tests.Reporter.Http
{
    public class HttpClientHandlerFactoryTest
    {
        [Fact]
        public void ShouldCreateHandler()
        {
            var configuration = new ConfigurationBuilder().Build();

            var factory = new HttpClientHandlerFactory(configuration);

            factory.Create().Should().NotBeNull();
        }

        [Fact]
        public void ShouldThrowExceptionWhenConfigurationIsNull()
        {
            Action act = () => new HttpClientHandlerFactory(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("host", "http://host/")]
        [InlineData("host:8080", "http://host:8080/")]
        [InlineData("http://host", "http://host/")]
        [InlineData("https://host", "https://host/")]
        public void ShouldParseProxyWithoutCredentials(string actual, string expected)
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["server:proxy:url"] = actual;

            var handler = new HttpClientHandlerFactory(configuration).Create();

            handler.Proxy.Should().NotBeNull();
            (handler.Proxy as WebProxy).Address.Should().Be(expected);
        }

        [Fact]
        public void ShouldParseProxyWithCredentials()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["server:proxy:url"] = "host";

            configuration.Properties["server:proxy:username"] = "user1";
            configuration.Properties["server:proxy:domain"] = "domain1";
            configuration.Properties["server:proxy:password"] = "password1";

            var handler = new HttpClientHandlerFactory(configuration).Create();

            handler.Proxy.Credentials.Should().NotBeNull();
            var creds = handler.Proxy.Credentials as NetworkCredential;
            creds.UserName.Should().Be("user1");
            creds.Domain.Should().Be("domain1");
            creds.Password.Should().Be("password1");
        }
    }
}
