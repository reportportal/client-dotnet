using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Shared.Internal.Logging
{
    public interface ITraceLogger
    {
        void Info(string message);

        void Verbose(string message);

        void Warn(string message);

        void Error(string message);
    }
}
