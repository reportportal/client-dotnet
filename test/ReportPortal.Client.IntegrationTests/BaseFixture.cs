using System;

namespace ReportPortal.Client.IntegrationTests
{
    public class BaseFixture
    {
        //protected static readonly string Username = "default";
        //protected static readonly string ProjectName = "default_personal";
        //protected readonly Service Service = new Service(new Uri("https://demo.reportportal.io/api/v1"), ProjectName, "695bb79b-0419-472f-bb7c-dd1e6e932a4f");
        protected static readonly string Username = "default";
        protected static readonly string ProjectName = "default_personal";
        protected readonly Service Service = new Service(new Uri("http://localhost:8080/api/v1"), ProjectName, "d562c898-7705-49a4-be6d-17a8121715fa");
    }
}
