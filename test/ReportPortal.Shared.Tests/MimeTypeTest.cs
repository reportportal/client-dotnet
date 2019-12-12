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
            Assert.Equal(expectedMime, MimeTypes.MimeTypeMap.GetMimeType(fileExtension));
        }
    }
}
