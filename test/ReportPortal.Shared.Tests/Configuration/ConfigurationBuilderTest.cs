using FluentAssertions;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Configuration.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace ReportPortal.Shared.Tests.Configuration
{
    // todo: isolate tests which setting environment variables, it affects others tests
    public class ConfigurationBuilderTest
    {
        [Theory]
        [InlineData("REPORTPORTAL_ABC", "ABC", "test")]
        [InlineData("REPORTPORTAL_ABC", "abc", "test")]
        [InlineData("REPORTPORTAL_A_B", "a:b", "test")]
        [InlineData("REPORTPORTAL__A__B__C", "a:b:c", "test")]
        [InlineData("RP__A__B__C", "a:b:c", "test")]
        [InlineData("RP_ABC", "ABC", "test")]
        [InlineData("RP_ABC", "abc", "test")]
        [InlineData("RP_A_B", "a:b", "test")]
        public void ShouldGetEnvironmentVariable(string paramFullName, string paramName, string paramValue)
        {
            Environment.SetEnvironmentVariable(paramFullName, paramValue);

            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            var variable = config.GetValue<string>(paramName);

            Assert.Equal(paramValue, variable);

            Environment.SetEnvironmentVariable(paramFullName, null);
        }

        [Fact]
        public void ShouldGetVariableFromJsonFile()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, @"{""prop1"": ""value1"", ""prop2"": ""value2""}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build();

            var variable1 = config.GetValue<string>("prop1");
            Assert.Equal("value1", variable1);
            var variable2 = config.GetValue<string>("prop2");
            Assert.Equal("value2", variable2);
        }

        [Fact]
        public void ShouldGetVariableWithNewLineFromJsonFile()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, @"{""prop1"": ""line1\nline2""}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build();

            var variable = config.GetValue<string>("prop1");
            Assert.Equal("line1\nline2", variable);
        }

        [Fact]
        public void ShouldNotAffectNextVariableWhenNewLineInPreviousFromJsonFile()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, @"{""prop1"": ""line1\nline2"",""sibling_prop1"": {""prop1"": ""line11\nline22""}, ""prop2"": ""line3\nline4"", ""prop3"": ""line33\nline44""}");

            var config = new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build();

            var variable1 = config.GetValue<string>("prop1");
            Assert.Equal("line1\nline2", variable1);

            var sibling_variable11 = config.GetValue<string>("sibling_prop1:prop1");
            Assert.Equal("line11\nline22", sibling_variable11);

            var variable2 = config.GetValue<string>("prop2");
            Assert.Equal("line3\nline4", variable2);

            var variable3 = config.GetValue<string>("prop3");
            Assert.Equal("line33\nline44", variable3);
        }

        [Fact]
        public void ShouldOverrideVariableWithTheSameProperty()
        {
            Environment.SetEnvironmentVariable("REPORTPORTAL_prop1", "value1");

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, @"{""prop1"": ""over_value1""}");

            var config = new ConfigurationBuilder().AddEnvironmentVariables().AddJsonFile(filePath: tempFile).Build();

            var value = config.GetValue<string>("prop1");

            Assert.Equal("over_value1", value);

            Environment.SetEnvironmentVariable("REPORTPORTAL_prop1", null);
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

            Assert.Equal(1, config.Properties.Count);

            var variable = config.GetValue<string>("prop1:prop2");
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

            Assert.Equal(2, config.Properties.Count);

            var variable1 = config.GetValue<string>("prop1");
            Assert.Equal("value1;value2;", variable1);

            var variable2 = config.GetValue<string>("prop2");
            Assert.Equal("value11;value22;", variable2);
        }

        [Fact]
        public void ShouldJsonFileNameBeCaseInsensitive()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile.ToLowerInvariant(), "{ \"a\": true }");

            var config = new ConfigurationBuilder().AddJsonFile(Path.Combine(Path.GetDirectoryName(tempFile), Path.GetFileName(tempFile).ToUpperInvariant())).Build();
            config.GetValue<bool>("a").Should().BeTrue();
        }

        [Fact]
        public void ShouldRaiseExceptionIfJsonIsIncorrect()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, "Bh}");

            Assert.ThrowsAny<System.Text.Json.JsonException>(() => new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build());
        }

        // this test might pass in future when we will use oanother ser/derser library
        [Fact]
        public void MightRaiseExceptionIfJsonIsNotStandartized()
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, "{\n// this is comment?\n}");

            Assert.ThrowsAny<System.Text.Json.JsonException>(() => new ConfigurationBuilder().AddJsonFile(filePath: tempFile).Build());
        }

        [Fact]
        public void ShouldRaiseExceptionIfVariableNotFound()
        {
            var config = new ConfigurationBuilder().Build();

            var exp = Assert.Throws<KeyNotFoundException>(() => config.GetValue<string>("someproperty"));
            Assert.Contains("someproperty", exp.Message);
        }

        [Fact]
        public void ShouldReturnDefaultIfVariableNotFound()
        {
            var config = new ConfigurationBuilder().Build();

            var a = config.GetValue("a", "abc");

            Assert.Equal("abc", a);
        }

        [Fact]
        public void ShouldReturnNullAsDefaultIfVariableNotFound()
        {
            var config = new ConfigurationBuilder().Build();

            var a = config.GetValue<string>("a", null);

            a.Should().BeNull();
        }

        [Fact]
        public void ShouldRaiseExceptionIfListNotFound()
        {
            var config = new ConfigurationBuilder().Build();

            var exp = Assert.Throws<KeyNotFoundException>(() => config.GetValues<string>("someproperty"));
            Assert.Contains("someproperty", exp.Message);
        }

        [Fact]
        public void ShouldReturnDefaultIfListNotFound()
        {
            var config = new ConfigurationBuilder().Build();

            var list = config.GetValues("a", new List<string> { "abc" });

            Assert.Equal(new List<string> { "abc" }, list);
        }

        [Fact]
        public void ShouldReturNullAsDefaultIfListNotFound()
        {
            var config = new ConfigurationBuilder().Build();

            var list = config.GetValues<string>("a", null);

            list.Should().BeNull();
        }

        [Theory]
        [InlineData("true")]
        [InlineData("tRue")]
        [InlineData("yes")]
        [InlineData("y")]
        [InlineData("1")]
        public void ShouldReturnTrueBooleanValue(string value)
        {
            var config = new ConfigurationBuilder().Build();
            config.Properties["a"] = value;
            Assert.True(config.GetValue<bool>("a"));
        }

        [Theory]
        [InlineData("false")]
        [InlineData("fAlse")]
        [InlineData("no")]
        [InlineData("n")]
        [InlineData("0")]
        public void ShouldReturnFalseBooleanValue(string value)
        {
            var config = new ConfigurationBuilder().Build();
            config.Properties["a"] = value;
            Assert.False(config.GetValue<bool>("a"));
        }

        [Fact]
        public void ShouldNotReturnDefaultIfListFound()
        {
            var config = new ConfigurationBuilder().Build();
            config.Properties["a"] = "a1;a2";

            var list = config.GetValues("a", new List<string> { "abc" });

            list.Should().BeEquivalentTo(new List<string> { "a1", "a2" });
        }

        [Fact]
        public void ShouldMergeValuesIfStartsWithPlus()
        {
            Environment.SetEnvironmentVariable("REPORTPORTAL_A", "++value1");

            var config = new ConfigurationBuilder().AddEnvironmentVariables().AddEnvironmentVariables().Build();

            var variable = config.GetValue<string>("A");

            Assert.Equal("value1value1", variable);

            Environment.SetEnvironmentVariable("REPORTPORTAL_A", null);
        }

        [Fact]
        public void ShouldMergeListOfValuesIfStartsWithPlus()
        {
            Environment.SetEnvironmentVariable("REPORTPORTAL_A", "++value1;");

            var config = new ConfigurationBuilder().AddEnvironmentVariables().AddEnvironmentVariables().Build();

            var list = config.GetValues<string>("A");

            Assert.Equal(new List<string> { "value1", "value1" }, list);

            Environment.SetEnvironmentVariable("REPORTPORTAL_A", null);
        }

        [Fact]
        public void ShouldWorkWithCustomDelimeterAndPrefix()
        {
            Environment.SetEnvironmentVariable("CUSTOMPREFIX_prop1-prop2", "value1");

            var config = new ConfigurationBuilder().Add(new EnvironmentVariablesConfigurationProvider("CUSTOMPREFIX_", "-", EnvironmentVariableTarget.Process)).Build();

            var value = config.GetValue<string>("prop1:prop2");

            Assert.Equal("value1", value);

            Environment.SetEnvironmentVariable("CUSTOMPREFIX_prop1-prop2", null);
        }

        [Fact]
        public void ShouldNotThrowExceptionIfJsonFileNotFound()
        {
            new ConfigurationBuilder().AddJsonFile("qwe.json").Build();
        }

        [Fact]
        public void ShouldThrowExceptionIfJsonFileNotFound()
        {
            Assert.Throws<FileLoadException>(() => new ConfigurationBuilder().AddJsonFile("qwe.json", optional: false).Build());
        }

        [Fact]
        public void ShouldGetKeyValue()
        {
            var config = new ConfigurationBuilder().Build();
            config.Properties["a"] = "k1:v1";

            config.GetKeyValues<string>("a").Should().BeEquivalentTo(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("k1", "v1") });
        }

        [Fact]
        public void ShouldGetTrimmedKeyValues()
        {
            var config = new ConfigurationBuilder().Build();
            config.Properties["a"] = "k1:v1; k2 : v2 ";

            config.GetKeyValues<string>("a").Should().BeEquivalentTo(
                new List<KeyValuePair<string, string>>(){
                    new KeyValuePair<string, string>("k1", "v1"),
                    new KeyValuePair<string, string>("k2", "v2")
                });
        }

        [Fact]
        public void ShouldGetKeyValues()
        {
            var config = new ConfigurationBuilder().Build();
            config.Properties["a"] = "k1:v1;k2:v2";

            config.GetKeyValues<string>("a").Should().BeEquivalentTo(
                new List<KeyValuePair<string, string>>(){
                    new KeyValuePair<string, string>("k1", "v1"),
                    new KeyValuePair<string, string>("k2", "v2")
                });
        }

        [Fact]
        public void ShouldGetKeyValuesWithEmptyKey()
        {
            var config = new ConfigurationBuilder().Build();
            config.Properties["a"] = "k1:v1;v2;k3:v3:v3";

            config.GetKeyValues<string>("A").Should().BeEquivalentTo(
                new List<KeyValuePair<string, string>>(){
                    new KeyValuePair<string, string>("k1", "v1"),
                    new KeyValuePair<string, string>("", "v2"),
                    new KeyValuePair<string, string>("k3", "v3:v3")
                });
        }

        [Fact]
        public void ShouldUseDefaults()
        {
            var dir = Directory.CreateDirectory(Path.GetRandomFileName());
            File.AppendAllText(dir + "/ReportPortal_prop1", "value1");
            File.AppendAllText(dir + "/ReportPortal.config.json", @"{""prop2"": ""value2""}");

            var config = new ConfigurationBuilder().AddDefaults(dir.FullName).Build();

            config.Properties.Should().HaveCountGreaterOrEqualTo(2).And.ContainKeys("prop1", "prop2");

            dir.Delete(true);
        }
    }
}
