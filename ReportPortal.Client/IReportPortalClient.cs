using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Client
{
    public interface IReportPortalClient
    {
        string ProjectName { get; set; }

        Uri BaseUri { get; set; }
    }
}
