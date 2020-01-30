using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using System;
using System.Runtime.Serialization;
using Xunit;

namespace ReportPortal.Client.IntegrationTests.Serialization
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

        [Fact]
        public void ShouldDeserializeWithEscapedNewLine()
        {
            var json = "{\"P1\": \"abc\\nabc\"}";
            var a = ModelSerializer.Deserialize<A>(json);
            Assert.Equal("abc\nabc", a.P1);
        }

        [DataContract]
        class A
        {
            [DataMember]
            public string P1 { get; set; }
        }
    }
}
