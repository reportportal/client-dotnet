using Moq;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Shared.Internal.Delegating;
using System;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Tests.Helpers
{
    class MockRequestExecuterFactoryBuilder
    {
        public Mock<IRequestExecuterFactory> Build()
        {
            var requestExecuter = new Mock<IRequestExecuter>();

            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<LaunchCreatedResponse>>>())).Returns<Func<Task<LaunchCreatedResponse>>>(v => v.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<TestItemCreatedResponse>>>())).Returns<Func<Task<TestItemCreatedResponse>>>(v => v.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<LogItemCreatedResponse>>>())).Returns<Func<Task<LogItemCreatedResponse>>>(v => v.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<MessageResponse>>>())).Returns<Func<Task<MessageResponse>>>(v => v.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<LaunchFinishedResponse>>>())).Returns<Func<Task<LaunchFinishedResponse>>>(v => v.Invoke());

            var requestExecuterFactory = new Mock<IRequestExecuterFactory>();
            requestExecuterFactory.Setup(f => f.Create()).Returns(requestExecuter.Object);

            return requestExecuterFactory;
        }
    }
}
