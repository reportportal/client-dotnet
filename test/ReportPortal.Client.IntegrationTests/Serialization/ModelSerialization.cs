using FluentAssertions;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.Serialization
{
    public class ModelSerialization
    {
        [Fact]
        public void ShouldThrowExceptionIfIncorrectJson()
        {
            var json = "<abc />";
            using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                Func<Task> act = async () => await ModelSerializer.DeserializeAsync<MessageResponse>(reader);
                act.Should().Throw<Exception>();
            }
        }

        [Fact]
        public async Task ShouldDeserializeWithEscapedNewLine()
        {
            var json = "{\"message\": \"abc\\nabc\"}";
            using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var message = await ModelSerializer.DeserializeAsync<MessageResponse>(reader);
                Assert.Equal("abc\nabc", message.Info);
            }
        }
    }
}
