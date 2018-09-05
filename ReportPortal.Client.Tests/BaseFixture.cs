using System;

namespace ReportPortal.Client.Tests
{
    public class BaseFixture
    {
        protected static readonly string Username = "vsonchik";
        protected readonly Service Service = new Service(new Uri("http://10.129.107.10/api/v1"), "mysite-ui-automation", "d7c8053c-b706-4fdb-81e5-8b8a1e24dae7");
    }
}
