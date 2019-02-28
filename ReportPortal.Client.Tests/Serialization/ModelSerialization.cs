using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using System;
using Xunit;

namespace ReportPortal.Client.Tests.Serialization
{
    public class ModelSerialization
    {
        [Fact]
        public void ShouldThrowExceptionIfIncorrectJson()
        {
            var json = "<abc />";
            var exp = Assert.ThrowsAny<Exception>(() => ModelSerializer.Deserialize<Message>(json));
            Assert.Contains(json, exp.Message);
            Assert.NotNull(exp.InnerException);
        }
    }
}
