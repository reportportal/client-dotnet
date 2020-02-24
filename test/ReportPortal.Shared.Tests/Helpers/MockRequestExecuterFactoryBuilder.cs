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

            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<LaunchCreatedResponse>>>(), It.IsAny<Action<Exception>>())).Returns<Func<Task<LaunchCreatedResponse>>, Action>((f, c) => f.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<TestItemCreatedResponse>>>(), It.IsAny<Action<Exception>>())).Returns<Func<Task<TestItemCreatedResponse>>, Action>((f, c) => f.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<LogItemCreatedResponse>>>(), It.IsAny<Action<Exception>>())).Returns<Func<Task<LogItemCreatedResponse>>, Action>((f, c) => f.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<MessageResponse>>>(), It.IsAny<Action<Exception>>())).Returns<Func<Task<MessageResponse>>, Action>((f, c) => f.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<LaunchFinishedResponse>>>(), It.IsAny<Action<Exception>>())).Returns<Func<Task<LaunchFinishedResponse>>, Action>((f, c) => f.Invoke());

            var requestExecuterFactory = new Mock<IRequestExecuterFactory>();
            requestExecuterFactory.Setup(f => f.Create()).Returns(requestExecuter.Object);

            return requestExecuterFactory;
        }
    }
}
