using System;

namespace ReportPortal.Client.IntegrationTests
{
    public class BaseFixture
    {
        //protected static readonly string Username = "ci_check_net_client";
        //protected readonly Service Service = new Service(new Uri("https://rp.epam.com/api/v1"), "ci-agents-checks", "ba7eb7c8-7b33-42f6-8cf0-e9cd26e717f4");

        protected static readonly string Username = "default";
        protected readonly Service Service = new Service(new Uri("https://demo.reportportal.io/api/v1"), "default_personal", "c0e41c48-f82c-4c6d-945d-6559e2a6c8e5");

        //protected static readonly string Username = "ci_check_net_client";
        //protected readonly Service Service = new Service(new Uri("https://alpha.reportportal.io/api/v1"), "default_personal", "cd9c39d6-c9a2-45b0-8e48-c4f0151114d0");
    }
}
