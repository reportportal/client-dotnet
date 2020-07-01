using FluentAssertions;
using Moq;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Shared.Execution;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.Commands;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ReportPortal.Shared.Tests.Execution
{
    public class TestMetadataFixture
    {
        [Fact]
        public void ShouldNotReturnNullableAttributes()
        {
            var testContext = new TestContext(new ExtensionManager(), new CommandsSource(null));
            testContext.Metadata.Attributes.Should().BeEmpty();
        }

        [Fact]
        public void ShouldRaiseOnGetAttributes()
        {
            var mockListener = new Mock<ICommandsListener>();
            mockListener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.TestCommandsSource.OnGetTestAttributes += (ctx, args) =>
                    args.Attributes.Add(new ItemAttribute());
            });

            var listener = mockListener.Object;

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener);

            var testContext = new TestContext(extensionManager, new CommandsSource(new List<ICommandsListener> { mockListener.Object }));

            var attributes = testContext.Metadata.Attributes;
            attributes.Should().HaveCount(1);
            testContext.Metadata.Attributes.Should().BeSameAs(attributes);
        }

        [Fact]
        public void ShouldRaiseOnAddAttributes()
        {
            ICollection<ItemAttribute> attributesToAdd = null;

            var mockListener = new Mock<ICommandsListener>();
            mockListener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.TestCommandsSource.OnAddTestAttributes += (ctx, args) => attributesToAdd = args.Attributes;
            });

            var listener = mockListener.Object;

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener);

            var testContext = new TestContext(extensionManager, new CommandsSource(new List<ICommandsListener> { mockListener.Object }));

            testContext.Metadata.Attributes.Add(new ItemAttribute());
            attributesToAdd.Should().HaveCount(1);
            testContext.Metadata.Attributes.Should().BeEquivalentTo(attributesToAdd);
        }

        [Fact]
        public void ShouldRaiseOnRemoveAttributes()
        {
            ICollection<ItemAttribute> attributesToRemove = null;

            var mockListener = new Mock<ICommandsListener>();
            mockListener.Setup(o => o.Initialize(It.IsAny<ICommandsSource>())).Callback<ICommandsSource>(s =>
            {
                s.TestCommandsSource.OnRemoveTestAttributes += (ctx, args) => attributesToRemove = args.Attributes;
            });

            var listener = mockListener.Object;

            var extensionManager = new ExtensionManager();
            extensionManager.CommandsListeners.Add(listener);

            var testContext = new TestContext(extensionManager, new CommandsSource(new List<ICommandsListener> { mockListener.Object }));

            var attr = new ItemAttribute();
            testContext.Metadata.Attributes.Add(attr);
            testContext.Metadata.Attributes.Remove(attr);
            attributesToRemove.Should().HaveCount(1);
            attributesToRemove.First().Should().BeEquivalentTo(attr);
        }
    }
}
