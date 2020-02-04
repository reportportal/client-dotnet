using System;

namespace ReportPortal.Client.Http
{
    public class ReportPortalException : Exception
    {
        public ReportPortalException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
