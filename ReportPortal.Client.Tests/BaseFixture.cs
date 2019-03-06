using System;

namespace ReportPortal.Client.Tests
{
    public class BaseFixture
    {
        protected static readonly string Username = "ci_check_net_client";
        protected readonly ReportPortalClient Service = new ReportPortalClient(new Uri("https://rp.epam.com/api/v1"), "ci-agents-checks", "7853c7a9-7f27-43ea-835a-cab01355fd17");
    }
}
