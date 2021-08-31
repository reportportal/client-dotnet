﻿using System;

namespace ReportPortal.Client.IntegrationTests
{
    public class BaseFixture
    {
        //protected static readonly string Username = "default";
        //protected static readonly string ProjectName = "default_personal";
        //protected readonly Service Service = new Service(new Uri("https://demo.reportportal.io/api/v1"), ProjectName, "6475bbef-a343-4f6a-add2-3dfc5e4af7bc");

        protected static readonly string Username = "default";
        protected static readonly string ProjectName = "default_personal";
        protected readonly Service Service = new Service(new Uri("http://localhost:8080/api/v1"), ProjectName, "694b1c68-a3dd-4fd5-a752-8cd233cb2733");
    }
}
