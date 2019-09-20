using Moq;
using ReportPortal.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Tests.Helpers
{
    class MockServiceBuilder
    {
        public Mock<Service> Build()
        {
            var service = new Mock<Service>(new Uri("http://abc.com"), It.IsAny<string>(), It.IsAny<string>());

            service.Setup(s => s.StartLaunchAsync(It.IsAny<Client.Requests.StartLaunchRequest>())).Returns(Task.FromResult(new Client.Models.Launch()));

            service.Setup(s => s.StartTestItemAsync(It.IsAny<Client.Requests.StartTestItemRequest>())).Returns(Task.FromResult(new Client.Models.TestItem()));
            service.Setup(s => s.StartTestItemAsync(null, It.IsAny<Client.Requests.StartTestItemRequest>())).Returns(Task.FromResult(new Client.Models.TestItem()));

            return service;
        }
    }
}
