using System;

namespace ReportPortal.Client.Tests.Base
{
    public class BaseFixture
    {
        protected static readonly string Username = "ci_check_net_client";
        protected readonly Service Service = new Service(new Uri("https://rp.epam.com/api/v1"), "default_project", "7853c7a9-7f27-43ea-835a-cab01355fd17");
    }
}
