using FluentAssertions;
using ReportPortal.Shared.Configuration.Providers;
using System;
using System.IO;
using Xunit;

namespace ReportPortal.Shared.Tests.Configuration
{
    public class DirectoryProbingFixture
    {
        [Fact]
        public void ShouldFindDirectory()
        {
            var dir = Directory.CreateDirectory(Path.GetRandomFileName());
            File.AppendAllText(Path.Combine(dir.FullName, "rp_a1"), "a1_value");

            var dirProvider = new DirectoryProbingConfigurationProvider(dir.FullName, "rP", "_", false);
            dirProvider.Load().Should().HaveCount(1).And.ContainKey("a1").WhichValue.Should().Be("a1_value");

            dir.Delete(true);
        }

        [Fact]
        public void ShouldTrimContent()
        {
            var dir = Directory.CreateDirectory(Path.GetRandomFileName());
            File.AppendAllText(Path.Combine(dir.FullName, "rp_a1"), $" a1_value {Environment.NewLine}");

            var dirProvider = new DirectoryProbingConfigurationProvider(dir.FullName, "rP", "_", false);
            dirProvider.Load().Should().HaveCount(1).And.ContainKey("a1").WhichValue.Should().Be("a1_value");

            dir.Delete(true);
        }

        [Fact]
        public void ShouldConsiderSeveralFiles()
        {
            var dir = Directory.CreateDirectory(Path.GetRandomFileName());
            File.AppendAllText(dir + "/rp_a1", "a1_value");
            File.AppendAllText(dir + "/rp_a2_b1", "a2_b1_value");

            var dirProvider = new DirectoryProbingConfigurationProvider(dir.FullName, "rp", "_", false);
            var properties = dirProvider.Load();
            properties.Should().HaveCount(2).And.ContainKeys("a1", "a2:b1");
            properties["a1"].Should().Be("a1_value");
            properties["a2:b1"].Should().Be("a2_b1_value");
            dir.Delete(true);
        }

        [Fact]
        public void ShouldSkipSomeFiles()
        {
            var dir = Directory.CreateDirectory(Path.GetRandomFileName());
            File.AppendAllText(dir + "/rp.a1.eXe", "a1_value");
            File.AppendAllText(dir + "/rp_a1.dll", "a1_value");
            File.AppendAllText(dir + "/rp_a1.log", "a1_value");
            File.AppendAllText(dir + "/rp_a1.pdb", "a1_value");

            var dirProvider = new DirectoryProbingConfigurationProvider(dir.FullName, "rp", "_", false);
            dirProvider.Load().Should().BeEmpty();

            dir.Delete(true);
        }

        [Theory]
        [InlineData(null, "rp", "_")]
        [InlineData("ab", null, "_")]
        [InlineData("ab", "rp", null)]
        public void ShouldVerifyArguments(string dir, string prefix, string delimeter)
        {
            Action ctor = () => new DirectoryProbingConfigurationProvider(dir, prefix, delimeter, false);
            ctor.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void ShouldThrowIfDirectoryIsRequiredWhenLoading()
        {
            var provider = new DirectoryProbingConfigurationProvider("some_unexisting_dir", "rp", "_", optional: false);
            provider.Invoking(p => p.Load()).Should().ThrowExactly<DirectoryNotFoundException>().Which.Message.Should().Contain("some_unexisting_dir");
        }

        [Fact]
        public void ShouldBeSilentIfDirectoryIsOptional()
        {
            var provider = new DirectoryProbingConfigurationProvider("some_unexisting_dir", "rp", "_", optional: true);
            var properties = provider.Load();
            properties.Should().BeEmpty();
        }
    }
}
