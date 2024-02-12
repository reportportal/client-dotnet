using FluentAssertions;
using ReportPortal.Shared.Converters;
using Xunit;

namespace ReportPortal.Shared.Tests.Converters
{
    public class ItemAttributeFixture
    {
        [Theory]
        [InlineData("k1:v1", "k1", "v1")]
        [InlineData("v1", null, "v1")]
        [InlineData(":v1", null, "v1")]
        [InlineData("v1:", null, "v1")]
        [InlineData("k1:v1:v2", "k1", "v1:v2")]
        public void ShouldConvertFromString(string tag, string expectedKey, string expectedValue)
        {
            var converter = new ItemAttributeConverter();
            var attr = converter.ConvertFrom(tag);

            attr.Key.Should().Be(expectedKey);
            attr.Value.Should().Be(expectedValue);
        }

        [Fact]
        public void ShouldUseCustomOptions()
        {
            var converter = new ItemAttributeConverter();
            var attr = converter.ConvertFrom("v1", (opt) => { opt.UndefinedKey = "abc"; });

            attr.Key.Should().Be("abc");
            attr.Value.Should().Be("v1");
        }
    }
}
