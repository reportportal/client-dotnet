using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Embedded.Normalization;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Tests.Helpers;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Extensibility.Embedded.Normalization
{
    public class RequestNormalizerTest
    {
        private readonly IExtensionManager _extensionManager;

        public RequestNormalizerTest()
        {
            _extensionManager = new Shared.Extensibility.ExtensionManager();
            _extensionManager.ReportEventObservers.Add(new RequestNormalizer());
        }

        [Fact]
        public void ShouldTrimLaunchNameDuringStarting()
        {
            var launchReporter = new LaunchReporter(
                new MockServiceBuilder().Build().Object, null, null, _extensionManager);

            var request = new StartLaunchRequest
            {
                Name = new string('a', RequestNormalizer.MAX_LAUNCH_NAME_LENGTH + 1)
            };

            launchReporter.Start(request);

            launchReporter.Sync();

            request.Name.Should().HaveLength(RequestNormalizer.MAX_LAUNCH_NAME_LENGTH);
        }

        [Fact]
        public void ShouldTrimLaunchAttributesDuringStarting()
        {
            var launchReporter = new LaunchReporter(
                new MockServiceBuilder().Build().Object, null, null, _extensionManager);

            var request = new StartLaunchRequest
            {
                Attributes = new[]
                {
                    new ItemAttribute
                    { 
                        Key = new string('a', RequestNormalizer.MAX_ATTRIBUTE_KEY_LENGTH + 1),
                        Value = new string('b', RequestNormalizer.MAX_ATTRIBUTE_VALUE_LENGTH + 1)
                    },
                    new ItemAttribute 
                    {
                        Key = new string('a', RequestNormalizer.MAX_ATTRIBUTE_KEY_LENGTH * 2),
                        Value = new string('b', RequestNormalizer.MAX_ATTRIBUTE_VALUE_LENGTH * 2)
                    },
                }
            };

            launchReporter.Start(request);

            launchReporter.Sync();

            request.Attributes.Should().AllSatisfy(attribute =>
            {
                attribute.Key.Should().HaveLength(RequestNormalizer.MAX_ATTRIBUTE_KEY_LENGTH);
                attribute.Value.Should().HaveLength(RequestNormalizer.MAX_ATTRIBUTE_VALUE_LENGTH);
            });
        }

        [Fact]
        public void ShouldTrimTestItemNameDuringStarting()
        {
            var service = new MockServiceBuilder().Build().Object;

            var launchReporter = new LaunchReporter(service, null, null, _extensionManager);
            launchReporter.Start(new StartLaunchRequest());

            var request = new StartTestItemRequest 
            {
                Name = new string('a', RequestNormalizer.MAX_TEST_ITEM_NAME_LENGTH + 1)
            };

            var testReporter = launchReporter.StartChildTestReporter(request);

            testReporter.Sync();

            request.Name.Should().HaveLength(RequestNormalizer.MAX_TEST_ITEM_NAME_LENGTH);
        }

        [Fact]
        public void ShouldTrimTestItemAttributesDuringStarting()
        {
            var service = new MockServiceBuilder().Build().Object;

            var launchReporter = new LaunchReporter(service, null, null, _extensionManager);

            launchReporter.Start(new StartLaunchRequest());

            var request = new StartTestItemRequest
            {
                Attributes = new[]
                {
                    new ItemAttribute
                    {
                        Key = new string('a', RequestNormalizer.MAX_ATTRIBUTE_KEY_LENGTH + 1),
                        Value = new string('b', RequestNormalizer.MAX_ATTRIBUTE_VALUE_LENGTH + 1) 
                    },
                    new ItemAttribute 
                    {
                        Key = new string('a', RequestNormalizer.MAX_ATTRIBUTE_KEY_LENGTH * 2),
                        Value = new string('b', RequestNormalizer.MAX_ATTRIBUTE_VALUE_LENGTH * 2)
                    },
                }
            };

            var testReporter = launchReporter.StartChildTestReporter(request);

            testReporter.Sync();

            request.Attributes.Should().AllSatisfy(attribute =>
            {
                attribute.Key.Should().HaveLength(RequestNormalizer.MAX_ATTRIBUTE_KEY_LENGTH);
                attribute.Value.Should().HaveLength(RequestNormalizer.MAX_ATTRIBUTE_VALUE_LENGTH);
            });
        }

        [Fact]
        public void ShouldTrimTestItemAttributesDuringFinishing()
        {
            var service = new MockServiceBuilder().Build().Object;

            var launchReporter = new LaunchReporter(service, null, null, _extensionManager);
            launchReporter.Start(new StartLaunchRequest());

            var testReporter = launchReporter.StartChildTestReporter(new StartTestItemRequest { });

            var request = new FinishTestItemRequest
            {
                Attributes = new[]
                {
                    new ItemAttribute
                    {
                        Key = new string('a', RequestNormalizer.MAX_ATTRIBUTE_KEY_LENGTH + 1),
                        Value = new string('b', RequestNormalizer.MAX_ATTRIBUTE_VALUE_LENGTH + 1)
                    },
                    new ItemAttribute
                    {
                        Key = new string('a', RequestNormalizer.MAX_ATTRIBUTE_KEY_LENGTH * 2),
                        Value = new string('b', RequestNormalizer.MAX_ATTRIBUTE_VALUE_LENGTH * 2)
                    },
                }
            };

            testReporter.Finish(request);

            testReporter.Sync();

            request.Attributes.Should().AllSatisfy(attribute =>
            {
                attribute.Key.Should().HaveLength(RequestNormalizer.MAX_ATTRIBUTE_KEY_LENGTH);
                attribute.Value.Should().HaveLength(RequestNormalizer.MAX_ATTRIBUTE_VALUE_LENGTH);
            });
        }

        [Fact]
        public void LaunchShouldCareOfLaunchFinishTime()
        {
            var launchStartTime = DateTime.UtcNow;

            var service = new MockServiceBuilder().Build();

            var launchReporter = new LaunchReporter(service.Object, null, null, _extensionManager);
            launchReporter.Start(new StartLaunchRequest() { StartTime = launchStartTime });
            launchReporter.Finish(new FinishLaunchRequest() { EndTime = launchStartTime.AddDays(-1) });
            launchReporter.Sync();

            launchReporter.Info.FinishTime.Should().Be(launchReporter.Info.StartTime);
        }

        [Fact]
        public void LaunchShouldCareOfSuiteStartTime()
        {
            var launchStartTime = DateTime.UtcNow;

            var service = new MockServiceBuilder().Build();

            var launchReporter = new LaunchReporter(service.Object, null, null, _extensionManager);

            launchReporter.Start(new StartLaunchRequest() { StartTime = launchStartTime });

            var request = new StartTestItemRequest() { StartTime = launchStartTime.AddDays(-1) };

            var testReporter = launchReporter.StartChildTestReporter(request);

            testReporter.Sync();

            request.StartTime.Should().Be(launchStartTime);
        }

        [Fact]
        public void LaunchShouldCareOfSuiteFinishTime()
        {
            var launchStartTime = DateTime.UtcNow;
            var service = new MockServiceBuilder().Build();

            var launchReporter = new LaunchReporter(service.Object, null, null, _extensionManager);

            launchReporter.Start(new StartLaunchRequest() { StartTime = launchStartTime });

            var request = new FinishTestItemRequest() { EndTime = launchStartTime.AddDays(-50) };

            var testReporter = launchReporter.StartChildTestReporter(
                new StartTestItemRequest { StartTime = launchStartTime.AddDays(-51) });

            testReporter.Finish(request);
            testReporter.Sync();

            request.EndTime.Should().Be(launchStartTime);
        }
    }
}
