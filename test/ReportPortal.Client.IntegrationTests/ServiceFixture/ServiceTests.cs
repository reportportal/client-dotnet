using ReportPortal.Client.Extensions;
using System;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.ServiceFixture
{
    public class ServiceFixture
    {
        [Theory]
        [InlineData("http://rp.epam.com", "http://rp.epam.com/api/")]
        [InlineData("http://rp.epam.com/", "http://rp.epam.com/api/")]
        [InlineData("http://rp.epam.com/api/v1", "http://rp.epam.com/api/")]
        [InlineData("http://rp.epam.com/api/v1/", "http://rp.epam.com/api/")]
        [InlineData("http://rp.epam.com/API/v1", "http://rp.epam.com/API/")]
        [InlineData("http://rp.epam.com/sub/API/v1", "http://rp.epam.com/sub/API/")]
        [InlineData("http://rp.epam.com/sub/API/v1/", "http://rp.epam.com/sub/API/")]
        public void ShouldAutomaticallyAppendApiPostfix(string url, string expectedUrl)
        {
            Assert.Equal(expectedUrl, new Uri(url).Normalize().ToString());
        }
    }
}
