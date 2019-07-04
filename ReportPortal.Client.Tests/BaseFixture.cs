using System;

namespace ReportPortal.Client.Tests
{
    public class BaseFixture
    {
        protected static readonly string Username = "ci_check_net_client";
        protected readonly Service Service = new Service(new Uri("https://rp.epam.com/api/v1"), "ci-agents-checks", "ba7eb7c8-7b33-42f6-8cf0-e9cd26e717f4");
    }
}
