using System;

namespace ReportPortal.Client.Tests
{
    public class BaseFixture
    {
        protected static readonly string Username = "autotesting";
        protected readonly Service Service = new Service(new Uri("http://192.168.71.223/api/v1/"), "autotesting_personal", "4176fb53-57ed-4b35-8dbb-d01b619f8629");
    }
}
