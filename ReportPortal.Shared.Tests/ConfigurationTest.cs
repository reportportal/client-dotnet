using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Configuration.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
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
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, @"{""prop1"": ""value1""}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build();

            var variable = config.GetValue<string>("prop1");
            Assert.Equal("value1", variable);
        }

        [Fact]
        public void ShouldGetIntegerVariableFromJsonFile()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, @"{""prop1"": 1}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build();

            var variable = config.GetValue<int>("prop1");
            Assert.Equal(1, variable);
        }

        [Fact]
        public void ShouldGetVariableSecondLevelFromJsonFile()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, "{\"prop1\": {\"prop2\": \"value2\"}}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build();

            Assert.Equal(1, config.Values.Count);

            var variable = config.GetValue<string>("prop1__prop2");
            Assert.Equal("value2", variable);
        }

        [Fact]
        public void ShouldGetListFromJsonFile()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, "{\"prop1\": [\"value1\", \"value2\"]}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build();

            var variable = config.GetValue<string>("prop1");
            Assert.Equal("value1;value2;", variable);

            var list = config.GetValues<string>("prop1");
            Assert.Equal(new List<string> { "value1", "value2" }, list);
        }

        [Fact]
        public void ShouldGetListOfStringsFromJsonFile()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, "{\"prop1\": [\"value1\", \"value2\"]}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build();

            var list = config.GetValues<string>("prop1");
            Assert.Equal(new List<string> { "value1", "value2" }, list);
        }

        [Fact]
        public void ShouldGetListOfIntegersFromJsonFile()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, "{\"prop1\": [1, 2]}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build();

            var list = config.GetValues<int>("prop1");
            Assert.Equal(new List<int> { 1, 2 }, list);
        }

        [Fact]
        public void ShouldGetSeveralListsFromJsonFile()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, "{\"prop1\": [\"value1\", \"value2\"], \"prop2\": [\"value11\", \"value22\"]}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build();

            Assert.Equal(2, config.Values.Count);

            var variable1 = config.GetValue<string>("prop1");
            Assert.Equal("value1;value2;", variable1);

            var variable2 = config.GetValue<string>("prop2");
            Assert.Equal("value11;value22;", variable2);
        }

        [Fact]
        public void ShouldRaiseExceptionIfJsonIsIncorrect()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, "Bh}");

            Assert.Throws<XmlException>(() => new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build());
        }

        [Fact]
        public void ShouldRaiseExceptionIfVariableNotFound()
        {
            var config = new ConfigurationBuilder().Build();

            Assert.Throws<KeyNotFoundException>(() => config.GetValue<string>("a"));
        }

        [Fact]
        public void ShouldReturnDefaultIfVariableNotFound()
        {
            var config = new ConfigurationBuilder().Build();

            var a = config.GetValue("a", "abc");

            Assert.Equal("abc", a);
        }

        [Fact]
        public void ShouldRaiseExceptionIfListNotFound()
        {
            var config = new ConfigurationBuilder().Build();

            Assert.Throws<KeyNotFoundException>(() => config.GetValues<string>("a"));
        }

        [Fact]
        public void ShouldReturnDefaultIfListNotFound()
        {
            var config = new ConfigurationBuilder().Build();

            var list = config.GetValues("a", new List<string> { "abc"});

            Assert.Equal(new List<string> { "abc" }, list);
        }
    }
}
