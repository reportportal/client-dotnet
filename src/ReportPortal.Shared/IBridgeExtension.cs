using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Shared
{
    public interface IBridgeExtension
    {
        int Order { get; }

        bool Handled { get; set; }

        void FormatLog(ref AddLogItemRequest logRequest);
    }
}
