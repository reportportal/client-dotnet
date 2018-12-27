using System;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    public class ConfigTest
    {
        [Fact]
        public void ShouldGetUuidEnvVariable()
        {
            Environment.SetEnvironmentVariable("RP_UUID", "ABC");

            var input = "{\"uuid\": \"{env:RP_UUID}\"}";

            var config = Client.Converters.ModelSerializer.Deserialize<Configuration.Authentication>(input);

            Assert.Equal("ABC", config.Uuid);
        }

        [Fact]
        public void ShouldRaiseNotFoundExceptionUuidEnvVariable()
        {
            var input = "{\"uuid\": \"{env:RP_UUID_NonExisting}\"}";

            var config = Client.Converters.ModelSerializer.Deserialize<Configuration.Authentication>(input);

            Assert.Throws<ArgumentNullException>(() => config.Uuid);
        }

        [Fact]
        public void ShouldGetUrlEnvVariable()
        {
            Environment.SetEnvironmentVariable("RP_URL", "http://abc.com/");

            var input = "{\"url\": \"{env:RP_URL}\"}";

            var config = Client.Converters.ModelSerializer.Deserialize<Configuration.Server>(input);

            Assert.Equal("http://abc.com/", config.Url.ToString());
        }

        [Fact]
        public void ShouldRaiseNotFoundExceptionUrlEnvVariable()
        {
            var input = "{\"url\": \"{env:RP_URL_NonExisting}\"}";

            var config = Client.Converters.ModelSerializer.Deserialize<Configuration.Server>(input);

            Assert.Throws<ArgumentNullException>(() => config.Url);
        }

        [Fact]
        public void ShouldGetProjectEnvVariable()
        {
            Environment.SetEnvironmentVariable("RP_PROJECT", "ABC");

            var input = "{\"project\": \"{env:RP_PROJECT}\"}";

            var config = Client.Converters.ModelSerializer.Deserialize<Configuration.Server>(input);

            Assert.Equal("ABC", config.Project);
        }

        [Fact]
        public void ShouldRaiseNotFoundExceptionProjectEnvVariable()
        {
            var input = "{\"project\": \"{env:RP_PROJECT_NonExisting}\"}";

            var config = Client.Converters.ModelSerializer.Deserialize<Configuration.Server>(input);

            Assert.Throws<ArgumentNullException>(() => config.Project);
        }
    }
}
