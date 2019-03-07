using System;

namespace ReportPortal.Client
{
    public interface IReportPortalClient
    {
        string ProjectName { get; set; }

        Uri BaseUri { get; set; }
    }
}
