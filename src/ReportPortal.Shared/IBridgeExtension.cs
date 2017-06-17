using ReportPortal.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Shared
{
    public interface IBridgeExtension
    {
        bool Log(LogLevel level, string message);
    }
}
