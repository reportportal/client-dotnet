using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Shared.Logging
{
    interface ILogScopeManager
    {
        ILogScope ActiveScope { get; set; }

        ILogScope RootScope { get; }
    }
}
