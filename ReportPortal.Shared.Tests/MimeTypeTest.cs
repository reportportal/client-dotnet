using ReportPortal.Client;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Shared.Tests
{
    public class MimeTypeTest
    {
        [Fact]
        public void GetMimeType()
        {
            Assert.Equal("image/png", MimeTypes.MimeTypeMap.GetMimeType(".png"));
            Assert.Equal("image/png", MimeTypes.MimeTypeMap.GetMimeType(".Png"));
            Assert.Equal("image/png", MimeTypes.MimeTypeMap.GetMimeType("png"));
            Assert.Equal("application/octet-stream", MimeTypes.MimeTypeMap.GetMimeType(".unknown"));
        }
    }
}
