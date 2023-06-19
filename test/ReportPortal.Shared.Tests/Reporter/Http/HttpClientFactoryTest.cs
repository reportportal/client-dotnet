using FluentAssertions;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Reporter.Http;
using System;
using System.Collections.Generic;
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
            configuration.Properties["server:apiKey"] = "123";

            var factory = new HttpClientFactory(configuration, _handler);

            var httpClient = factory.Create();
            httpClient.Should().NotBeNull();

            httpClient.BaseAddress.Should().Be("http://abc.com");
            httpClient.DefaultRequestHeaders.Authorization.Should().NotBeNull();
            httpClient.DefaultRequestHeaders.Authorization.Scheme.Should().Be("Bearer");
            httpClient.DefaultRequestHeaders.Authorization.Parameter.Should().Be("123");
        }

        [Fact]
        public void ShouldThrowExceptionWhenServerUrlIsMissing()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["server:apiKey"] = "123";

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

        [Fact]
        public void ShouldUseTimeout()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["server:url"] = "http://abc.com";
            configuration.Properties["server:apiKey"] = "123";
            configuration.Properties["server:timeout"] = 0.1;

            var factory = new HttpClientFactory(configuration, _handler);
            var httpClient = factory.Create();

            httpClient.Timeout.Should().Be(TimeSpan.FromSeconds(0.1));
        }

        [Fact]
        public void ShouldUseUuid()
        {
            var uuid = "12345";
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["Server:Url"] = "http://abc.com";
            configuration.Properties["Server:Project"] = "proj1";
            configuration.Properties["Server:Authentication:Uuid"] = uuid;

            var httpClient = new HttpClientFactory(configuration, _handler).Create();

            httpClient.Should().NotBeNull();
            httpClient.DefaultRequestHeaders.Authorization.Scheme.Should().Be("Bearer");
            httpClient.DefaultRequestHeaders.Authorization.Parameter.Should().Be(uuid);
        }

        [Fact]
        public void ShouldPreferApiKey()
        {
            var apiKey = "12345";
            var uuid = "54321";
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["Server:Url"] = "http://abc.com";
            configuration.Properties["Server:Project"] = "proj1";
            configuration.Properties["Server:Authentication:Uuid"] = uuid;
            configuration.Properties["Server:ApiKey"] = apiKey;

            var httpClient = new HttpClientFactory(configuration, _handler).Create();

            httpClient.Should().NotBeNull();
            httpClient.DefaultRequestHeaders.Authorization.Scheme.Should().Be("Bearer");
            httpClient.DefaultRequestHeaders.Authorization.Parameter.Should().Be(apiKey);
        }

        [Fact]
        public void ShouldThrowCorrectExceptionIfApiKeyOrUuidNotSet()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["Server:Url"] = "http://abc.com";
            configuration.Properties["Server:Project"] = "proj1";

            Action ctor = () => new HttpClientFactory(configuration, _handler).Create();
            ctor
                .Should()
                .ThrowExactly<KeyNotFoundException>(
                "Property 'Server:ApiKey' not found in the configuration. Make sure you have configured it properly."
                );
        }
    }
}
