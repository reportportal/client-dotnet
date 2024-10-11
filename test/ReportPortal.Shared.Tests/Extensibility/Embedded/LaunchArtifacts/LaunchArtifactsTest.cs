using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Embedded.LaunchArtifacts;
using ReportPortal.Shared.Tests.Helpers;
using System.IO;
using System.Threading;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.Embedded.LaunchArtifacts
{
    public class LaunchArtifactsTest
    {
        private readonly IExtensionManager _extensionManager;

        public LaunchArtifactsTest()
        {
            _extensionManager = new Shared.Extensibility.ExtensionManager();
            _extensionManager.ReportEventObservers.Add(new LaunchArtifactsEventsObserver());
        }

        [Fact]
        public void ShouldNotAttachArtifacts()
        {
            var client = new MockServiceBuilder().Build();

            var launchReporter = new LaunchReporterBuilder(client.Object).With(_extensionManager).Build(1, 0, 0);
            launchReporter.Sync();

            client.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest[]>(), default), Times.Never());
        }

        [Fact]
        public void ShouldAttachSingleArtifactByPath()
        {
            File.WriteAllBytes("test_file_1.txt", new byte[] { 1, 2, 3 });

            CreateLogItemRequest request = null;

            var client = new MockServiceBuilder().Build();
            client.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>(), default))
                .Callback<CreateLogItemRequest, CancellationToken>((a, b) => request = a);

            var config = new ConfigurationBuilder().Build();
            config.Properties["launch:artifacts"] = " test_file_1.txt ";

            var launchReporter = new LaunchReporterBuilder(client.Object).With(_extensionManager).WithConfiguration(config).Build(1, 0, 0);
            launchReporter.Sync();

            client.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>(), default), Times.Once);

            request.Should().NotBeNull();
            request.Text.Should().Be("test_file_1.txt");

            request.Attach.Should().NotBeNull();
            request.Attach.Name.Should().Be("test_file_1.txt");
            request.Attach.MimeType.Should().Be("text/plain");
            request.Attach.Data.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        }

        [Fact]
        public void ShouldAttachSeveralArtifactsByPath()
        {
            File.Create("test_file_1.txt").Close();
            File.Create("test_file_2.txt").Close();

            var client = new MockServiceBuilder().Build();

            var config = new ConfigurationBuilder().Build();
            config.Properties["launch:artifacts"] = "test_file_1.txt;test_file_2.txt";

            var launchReporter = new LaunchReporterBuilder(client.Object).With(_extensionManager).WithConfiguration(config).Build(1, 0, 0);
            launchReporter.Sync();

            client.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>(), default), Times.Exactly(2));
        }

        [Fact]
        public void ShouldAttachSingleArtifactByPattern()
        {
            File.WriteAllBytes("test_file_1.txt", new byte[] { 1, 2, 3 });

            CreateLogItemRequest request = null;

            var client = new MockServiceBuilder().Build();
            client.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>(), default))
                .Callback<CreateLogItemRequest, CancellationToken>((a, b) => request = a);

            var config = new ConfigurationBuilder().Build();
            config.Properties["launch:artifacts"] = "test_*_1.txt";

            var launchReporter = new LaunchReporterBuilder(client.Object).With(_extensionManager).WithConfiguration(config).Build(1, 0, 0);
            launchReporter.Sync();

            client.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>(), default), Times.Once);
        }

        [Fact]
        public void ShouldAttachSingleArtifactByDir()
        {
            Directory.CreateDirectory("a/b/c");
            File.WriteAllBytes("a/b/c/abc.txt", new byte[] { 1, 2, 3 });

            CreateLogItemRequest request = null;

            var client = new MockServiceBuilder().Build();
            client.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>(), default))
                .Callback<CreateLogItemRequest, CancellationToken>((a, b) => request = a);

            var config = new ConfigurationBuilder().Build();
            config.Properties["launch:artifacts"] = "a/b/c/*.txt";

            var launchReporter = new LaunchReporterBuilder(client.Object).With(_extensionManager).WithConfiguration(config).Build(1, 0, 0);
            launchReporter.Sync();

            client.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>(), default), Times.Once);
        }

        [Fact]
        public void ShouldAttachMessageWithException()
        {
            File.Create("test_file_open.txt"); // leaves it open

            CreateLogItemRequest request = null;

            var client = new MockServiceBuilder().Build();
            client.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>(), default))
                .Callback<CreateLogItemRequest, CancellationToken>((a, b) => request = a);

            var config = new ConfigurationBuilder().Build();
            config.Properties["launch:artifacts"] = "test_file_open.txt";

            var launchReporter = new LaunchReporterBuilder(client.Object).With(_extensionManager).WithConfiguration(config).Build(1, 0, 0);
            launchReporter.Sync();

            client.Verify(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>(), default), Times.Once);

            request.Text.Should().Contain("Couldn't read");
        }
    }
}
