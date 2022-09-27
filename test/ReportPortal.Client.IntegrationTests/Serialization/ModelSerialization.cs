using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.Serialization
{
    public class ModelSerialization
    {
        [Fact]
        public void ShouldThrowExceptionIfIncorrectJson()
        {
            var json = "<abc />";
            var exp = Assert.ThrowsAny<Exception>(() => ModelSerializer.Deserialize<MessageResponse>(json)); 
            Assert.NotNull(exp.InnerException);
        }

        [Fact]
        public void ShouldDeserializeWithEscapedNewLine()
        {
            var json = "{\"message\": \"abc\\nabc\"}";
            var message = ModelSerializer.Deserialize<MessageResponse>(json);
            Assert.Equal("abc\nabc", message.Info);
        }
    }
}
