using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Configuration.Providers;
using System;
using System.IO;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    public class ConfigurationTest
    {
        [Theory]
        [InlineData("REPORTPORTAL__ABC", "ABC", "test")]
        [InlineData("REPORTPORTAL__ABC", "abc", "test")]
        public void ShouldGetEnvironmentVariable(string paramFullName, string paramName, string paramValue)
        {
            Environment.SetEnvironmentVariable(paramFullName, paramValue);

            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            var variable = config.GetValue<string>(paramName);

            Assert.Equal(paramValue, variable);
        }

        [Fact]
        public void ShouldGetVariableFromJsonFile()
        {
            var filePath = Path.GetTempFileName();

            File.WriteAllText(filePath, "{\"abc\": \"test\"}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: filePath).Build();

            var variable = config.GetValue<string>("abc");
        }
    }
}
