using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Shared.Logging
{
    public interface ILogScope : IDisposable
    {
        void Info(string message);

        ILogScope BeginNewScope(string name);
    }
}
