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

            service.Setup(s => s.Launch.StartAsync(It.IsAny<StartLaunchRequest>(), default)).Returns(() => Task.FromResult(new LaunchCreatedResponse { Uuid = Guid.NewGuid().ToString() }));
            service.Setup(s => s.AsyncLaunch.StartAsync(It.IsAny<StartLaunchRequest>(), default)).Returns(() => Task.FromResult(new LaunchCreatedResponse { Uuid = Guid.NewGuid().ToString() }));

            service.Setup(s => s.TestItem.StartAsync(It.IsAny<StartTestItemRequest>(), default)).Returns(() => Task.FromResult(new TestItemCreatedResponse { Uuid = Guid.NewGuid().ToString() }));
            service.Setup(s => s.TestItem.StartAsync(It.IsAny<string>(), It.IsAny<StartTestItemRequest>(), default)).Returns(() => Task.FromResult(new TestItemCreatedResponse { Uuid = Guid.NewGuid().ToString() }));

            service.Setup(s => s.AsyncTestItem.StartAsync(It.IsAny<StartTestItemRequest>(), default)).Returns(() => Task.FromResult(new TestItemCreatedResponse { Uuid = Guid.NewGuid().ToString() }));
            service.Setup(s => s.AsyncTestItem.StartAsync(It.IsAny<string>(), It.IsAny<StartTestItemRequest>(), default)).Returns(() => Task.FromResult(new TestItemCreatedResponse { Uuid = Guid.NewGuid().ToString() }));

            service.Setup(s => s.LogItem.CreateAsync(It.IsAny<CreateLogItemRequest>(), default)).Returns(() => Task.FromResult(new LogItemCreatedResponse { Uuid = Guid.NewGuid().ToString() }));
            service.Setup(s => s.AsyncLogItem.CreateAsync(It.IsAny<CreateLogItemRequest>(), default)).Returns(() => Task.FromResult(new LogItemCreatedResponse { Uuid = Guid.NewGuid().ToString() }));

            service.Setup(s => s.TestItem.FinishAsync(It.IsAny<string>(), It.IsAny<FinishTestItemRequest>(), default)).Returns(() => Task.FromResult(new MessageResponse()));
            service.Setup(s => s.AsyncTestItem.FinishAsync(It.IsAny<string>(), It.IsAny<FinishTestItemRequest>(), default)).Returns(() => Task.FromResult(new MessageResponse()));

            service.Setup(s => s.Launch.FinishAsync(It.IsAny<string>(), It.IsAny<FinishLaunchRequest>(), default)).Returns(() => Task.FromResult(new LaunchFinishedResponse { Link = "http://server:80/path/to/launch" }));
            service.Setup(s => s.AsyncLaunch.FinishAsync(It.IsAny<string>(), It.IsAny<FinishLaunchRequest>(), default)).Returns(() => Task.FromResult(new LaunchFinishedResponse { Link = "http://server:80/path/to/launch" }));

            return service;
        }
    }
}
