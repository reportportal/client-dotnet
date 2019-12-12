using Moq;
using ReportPortal.Client.Models;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Internal.Delegating;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Tests.Helpers
{
    class MockRequestExecuterFactoryBuilder
    {
        public Mock<IRequestExecuterFactory> Build()
        {
            var requestExecuter = new Mock<IRequestExecuter>();

            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<Launch>>>())).Returns<Func<Task<Launch>>>(v => v.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<TestItem>>>())).Returns<Func<Task<TestItem>>>(v => v.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<LogItem>>>())).Returns<Func<Task<LogItem>>>(v => v.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<Message>>>())).Returns<Func<Task<Message>>>(v => v.Invoke());

            var requestExecuterFactory = new Mock<IRequestExecuterFactory>();
            requestExecuterFactory.Setup(f => f.Create(It.IsAny<IConfiguration>())).Returns(requestExecuter.Object);

            return requestExecuterFactory;
        }
    }
}
