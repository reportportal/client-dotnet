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

            File.WriteAllText(filePath, @"{""prop1"": ""value1""}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: filePath).Build();

            var variable = config.GetValue<string>("prop1");
            Assert.Equal("value1", variable);
        }

        [Fact]
        public void ShouldGetVariableSecondLevelFromJsonFile()
        {
            var filePath = Path.GetTempFileName();

            File.WriteAllText(filePath, "{\"prop1\": {\"prop2\": \"value2\"}}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: filePath).Build();

            Assert.Equal(1, config.Values.Count);

            var variable = config.GetValue<string>("prop1__prop2");
            Assert.Equal("value2", variable);
        }

        [Fact]
        public void ShouldGetListFromJsonFile()
        {
            var filePath = Path.GetTempFileName();

            File.WriteAllText(filePath, "{\"prop1\": [\"value1\", \"value2\"]}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: filePath).Build();

            var variable = config.GetValue<string>("prop1");
            Assert.Equal("value1;value2;", variable);
        }

        [Fact]
        public void ShouldGetSeveralListsFromJsonFile()
        {
            var filePath = Path.GetTempFileName();

            File.WriteAllText(filePath, "{\"prop1\": [\"value1\", \"value2\"], \"prop2\": [\"value11\", \"value22\"]}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: filePath).Build();

            Assert.Equal(2, config.Values.Count);

            var variable1 = config.GetValue<string>("prop1");
            Assert.Equal("value1;value2;", variable1);

            var variable2 = config.GetValue<string>("prop2");
            Assert.Equal("value11;value22;", variable2);
        }
    }
}
