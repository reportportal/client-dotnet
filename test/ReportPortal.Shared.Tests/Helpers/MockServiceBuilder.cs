using Moq;
using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Tests.Helpers
{
    class MockServiceBuilder
    {
        public Mock<IClientService> Build()
        {
            var service = new Mock<IClientService>();

            service.Setup(s => s.Launch.StartAsync(It.IsAny<StartLaunchRequest>())).Returns(() => new ValueTask<LaunchCreatedResponse>(new LaunchCreatedResponse { Uuid = Guid.NewGuid().ToString() }));

            service.Setup(s => s.TestItem.StartAsync(It.IsAny<StartTestItemRequest>())).Returns(() => new ValueTask<TestItemCreatedResponse>(new TestItemCreatedResponse { Uuid = Guid.NewGuid().ToString() }));
            service.Setup(s => s.TestItem.StartAsync(It.IsAny<string>(), It.IsAny<StartTestItemRequest>())).Returns(() => new ValueTask<TestItemCreatedResponse>(new TestItemCreatedResponse { Uuid = Guid.NewGuid().ToString() }));

            service.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>())).Returns(() => new ValueTask<LogItemCreatedResponse>(new LogItemCreatedResponse { Uuid = Guid.NewGuid().ToString() }));

            service.Setup(s => s.TestItem.FinishAsync(It.IsAny<string>(), It.IsAny<FinishTestItemRequest>())).Returns(() => new ValueTask<MessageResponse>(new MessageResponse()));

            service.Setup(s => s.Launch.FinishAsync(It.IsAny<string>(), It.IsAny<FinishLaunchRequest>())).Returns(() => new ValueTask<LaunchFinishedResponse>(new LaunchFinishedResponse { Link = "http://server:80/path/to/launch"}));

            return service;
        }
    }
}
