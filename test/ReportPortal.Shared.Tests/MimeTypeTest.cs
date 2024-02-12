using FluentAssertions;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    public class MimeTypeTest
    {
        [Theory]
        [InlineData("image/png", ".png")]
        [InlineData("image/png", ".Png")]
        [InlineData("image/png", "png")]
        [InlineData("application/octet-stream", ".unknown")]
        public void GetMimeType(string expectedMime, string fileExtension)
        {
            MimeTypes.MimeTypeMap.GetMimeType(fileExtension).Should().Be(expectedMime);
        }

        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            Action act = () => MimeTypes.MimeTypeMap.GetMimeType(null);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
