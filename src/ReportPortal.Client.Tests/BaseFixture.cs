using System;

namespace ReportPortal.Client.Tests
{
    public class BaseFixture
    {
        protected static readonly string Username = "default";
        protected readonly Service Service = new Service(new Uri("https://rp.epam.com/api/v1/"), "default_project", "45c00b4f-a893-4365-89be-8c1b89e30ffb");
    }
}
