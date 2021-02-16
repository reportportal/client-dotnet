using Moq;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Shared.Internal.Delegating;
using ReportPortal.Shared.Reporter.Statistics;
using System;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Tests.Helpers
{
    class MockRequestExecuterFactoryBuilder
    {
        public Mock<IRequestExecuterFactory> Build()
        {
            var requestExecuter = new Mock<IRequestExecuter>();

            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<LaunchCreatedResponse>>>(), It.IsAny<Action<Exception>>(), It.IsAny<IStatisticsCounter>())).Returns<Func<Task<LaunchCreatedResponse>>, Action, IStatisticsCounter>((f, c, s) => f.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<TestItemCreatedResponse>>>(), It.IsAny<Action<Exception>>(), It.IsAny<IStatisticsCounter>())).Returns<Func<Task<TestItemCreatedResponse>>, Action, IStatisticsCounter>((f, c, s) => f.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<LogItemCreatedResponse>>>(), It.IsAny<Action<Exception>>(), It.IsAny<IStatisticsCounter>())).Returns<Func<Task<LogItemCreatedResponse>>, Action, IStatisticsCounter>((f, c, s) => f.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<LogItemsCreatedResponse>>>(), It.IsAny<Action<Exception>>(), It.IsAny<IStatisticsCounter>())).Returns<Func<Task<LogItemsCreatedResponse>>, Action, IStatisticsCounter>((f, c, s) => f.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<MessageResponse>>>(), It.IsAny<Action<Exception>>(), It.IsAny<IStatisticsCounter>())).Returns<Func<Task<MessageResponse>>, Action, IStatisticsCounter>((f, c, s) => f.Invoke());
            requestExecuter.Setup(re => re.ExecuteAsync(It.IsAny<Func<Task<LaunchFinishedResponse>>>(), It.IsAny<Action<Exception>>(), It.IsAny<IStatisticsCounter>())).Returns<Func<Task<LaunchFinishedResponse>>, Action, IStatisticsCounter>((f, c, s) => f.Invoke());

            var requestExecuterFactory = new Mock<IRequestExecuterFactory>();
            requestExecuterFactory.Setup(f => f.Create()).Returns(requestExecuter.Object);

            return requestExecuterFactory;
        }
    }
}
