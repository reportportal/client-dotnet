using System;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.ServiceFixture
{
    public class ServiceFixture
    {
        [Theory]
        [InlineData("http://rp.epam.com", "http://rp.epam.com/api/v1")]
        [InlineData("http://rp.epam.com/", "http://rp.epam.com/api/v1")]
        [InlineData("http://rp.epam.com/api/v1", "http://rp.epam.com/api/v1")]
        [InlineData("http://rp.epam.com/api/v1/", "http://rp.epam.com/api/v1/")]
        [InlineData("http://rp.epam.com/API/v1", "http://rp.epam.com/API/v1")]
        public void ShouldAutomaticallyAppendApiPostfix(string url, string expectedUrl)
        {
            var service = new Service(new Uri(url), "", "");
            Assert.Equal(expectedUrl, service.BaseUri.ToString());
        }
    }
}
