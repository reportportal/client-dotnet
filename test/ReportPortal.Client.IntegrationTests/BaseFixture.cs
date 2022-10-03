using System;

namespace ReportPortal.Client.IntegrationTests
{
    public class BaseFixture
    {
        protected static readonly string Username = "default";
        protected static readonly string ProjectName = "default_personal";
        protected readonly Service Service = new Service(new Uri("https://demo.reportportal.io/api/v1"), ProjectName, "aeb09a3b-18ef-4e9e-b647-4645c0e9a74e");
        //protected static readonly string Username = "default";
        //protected static readonly string ProjectName = "default_personal";
        //protected readonly Service Service = new Service(new Uri("http://localhost:8080/api/v1"), ProjectName, "d562c898-7705-49a4-be6d-17a8121715fa");
    }
}
