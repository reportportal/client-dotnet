using FluentAssertions;
using Newtonsoft.Json.Linq;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Shared.Execution.Metadata;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Execution.Metadata
{
    public class MetaAttributeFixture
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldThrowExceptionWithIncorrectValue(string value)
        {
            Action act = () => new MetaAttribute(null, value);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("a:b", "a", "b")]
        [InlineData("a:b:c", "a", "b:c")]
        [InlineData(":b", null, "b")]
        [InlineData("b", null, "b")]
        public void ShouldBeAbleToParseString(string value, string expectedKey, string expectedValue)
        {
            var metaAttribute = MetaAttribute.Parse(value);
            metaAttribute.Key.Should().Be(expectedKey);
            metaAttribute.Value.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldNotBeAbleToParseIncorrectString(string value)
        {
            Action act = () => MetaAttribute.Parse(value);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void ShouldCastToItemAttribute()
        {
            var metaAttribute = new MetaAttribute("a", "b");
            ItemAttribute ia = metaAttribute;
            ia.Key.Should().Be("a");
            ia.Value.Should().Be("b");
        }

        [Fact]
        public void TwoMetaAttributesWithTheSameKeysAndValuesShouldBeEquals()
        {
            var firstAttribute = new MetaAttribute("a", "b");
            var secondAttriute = new MetaAttribute("a", "b");

            firstAttribute.Should().BeEquivalentTo(secondAttriute);
        }

        [Fact]
        public void TwoMetaAttributesWithDifferentKeysShouldNotBeEquals()
        {
            var firstAttribute = new MetaAttribute("a", "b");
            var secondAttriute = new MetaAttribute("c", "b");

            firstAttribute.Should().NotBeEquivalentTo(secondAttriute);
        }

        [Fact]
        public void TowMetaAttributesWithDifferentValuesShouldNotBeEquals()
        {
            var firstAttribute = new MetaAttribute("a", "b");
            var secondAttriute = new MetaAttribute("a", "c");

            firstAttribute.Should().NotBeEquivalentTo(secondAttriute);
        }
    }
}
