using System;

namespace ReportPortal.Client.IntegrationTests
{
    public class BaseFixture
    {
        // protected static readonly string Username = "ci_check_net_client";
        //protected readonly Service Service = new Service(new Uri("https://rp.epam.com/api/v1"), "ci-agents-checks", "ba7eb7c8-7b33-42f6-8cf0-e9cd26e717f4");

        //protected static readonly string Username = "ci_check_net_client";
        //protected readonly Service Service = new Service(new Uri("http://beta.demo.reportportal.io/api/v1"), "default_personal", "72606fd9-3b79-4ceb-b6e7-df6c5b2a94ae");

        protected static readonly string Username = "ci_check_net_client";
        protected readonly Service Service = new Service(new Uri("http://localhost:8080/api/v1"), "default_personal", "26f171a9-8bb2-45e8-9e0b-cd75bb7de670");
    }
}
