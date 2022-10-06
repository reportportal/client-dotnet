using FluentAssertions;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Reporter.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests.Reporter.Http
{
    public class ClientServiceBuilderTest
    {
        [Fact]
        public void ShouldBuildClientService()
        {
            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["Server:Url"] = "http://abc.com";
            configuration.Properties["Server:Project"] = "proj1";
            configuration.Properties["Server:Authentication:Uuid"] = "123";

            var builder = new ClientServiceBuilder(configuration);

            var client = builder.Build();

            client.Should().NotBeNull();
        }

        [Fact]
        public void ConfigurationShouldBeMandatory()
        {
            Action ctor = () => new ClientServiceBuilder(null);

            ctor.Should().ThrowExactly<ArgumentNullException>();
        }

        // ignore test for .net framework 452 and 46 because it always bypasses proxy for localhost
#if !NET46
        [Fact]
        public async Task ShouldUseProxy()
        {
            WireMock.Server.WireMockServer proxyServer = WireMock.Server.WireMockServer.Start();

            proxyServer
                .Given(WireMock.RequestBuilders.Request.Create())
                .RespondWith(WireMock.ResponseBuilders.Response.Create().WithBody("{\"email\": \"user@abc.com\"}"));

            var configuration = new ConfigurationBuilder().Build();
            configuration.Properties["Server:Url"] = $"http://abc.com";
            configuration.Properties["Server:Project"] = "proj1";
            configuration.Properties["Server:Authentication:Uuid"] = "123";

            configuration.Properties["Server:Proxy:Url"] = $"http://localhost:{proxyServer.Ports[0]}";

            var builder = new ClientServiceBuilder(configuration);

            var client = builder.Build();

            var user = await client.User.GetAsync();

            user.Email.Should().Be("user@abc.com");

            proxyServer.Stop();
        }
#endif

        [Fact]
        public void ShouldUseHttpClientFactory()
        {
            var configuration = new ConfigurationBuilder().Build();

            configuration.Properties["Server:Url"] = $"http://abc.com";
            configuration.Properties["Server:Project"] = "proj1";
            configuration.Properties["Server:Authentication:Uuid"] = "123";

            var builder = new ClientServiceBuilder(configuration);

            var clientHandler = new HttpClientHandler();

            var clientFactory = new MyCustomHttpClientFactory(configuration, clientHandler);

            builder.UseHttpClientFactory(clientFactory);

            builder.Build();

            clientFactory.IsInvoked.Should().BeTrue();
        }

        [Fact]
        public void ShouldUseHttpClientHandlerFactory()
        {
            var configuration = new ConfigurationBuilder().Build();

            configuration.Properties["Server:Url"] = $"http://abc.com";
            configuration.Properties["Server:Project"] = "proj1";
            configuration.Properties["Server:Authentication:Uuid"] = "123";

            var builder = new ClientServiceBuilder(configuration);

            var clientHandlerFactory = new MyCustomHttpClientHandlerFactory(configuration);

            builder.UseHttpClientHandlerFactory(clientHandlerFactory);

            builder.Build();

            clientHandlerFactory.IsInvoked.Should().BeTrue();
        }
    }

    class MyCustomHttpClientFactory : Shared.Reporter.Http.HttpClientFactory
    {
        public MyCustomHttpClientFactory(IConfiguration configuration, HttpClientHandler httpClientHandler) : base(configuration, httpClientHandler)
        {
        }

        public override HttpClient Create()
        {
            IsInvoked = true;

            return base.Create();
        }

        public bool IsInvoked { get; set; }
    }

    class MyCustomHttpClientHandlerFactory : HttpClientHandlerFactory
    {
        public MyCustomHttpClientHandlerFactory(IConfiguration configuration) : base(configuration)
        {
        }

        public override HttpClientHandler Create()
        {
            IsInvoked = true;

            return base.Create();
        }

        public bool IsInvoked { get; set; }
    }

}
