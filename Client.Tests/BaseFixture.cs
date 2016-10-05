using System;

namespace ReportPortal.Client.Tests
{
    public class BaseFixture
    {
        protected static readonly string Username = "default";
        protected readonly Service Service = new Service(new Uri("https://rp.epam.com/api/v1/"), "default_project", "aa19555c-c9ce-42eb-bb11-87757225d535");
    }
}
