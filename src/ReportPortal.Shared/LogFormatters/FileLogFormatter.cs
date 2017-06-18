using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Requests;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

namespace ReportPortal.Shared.LogFormatters
{
    public class FileLogFormatter : IBridgeExtension
    {
        public bool Handled { get; set; }

        public int Order
        {
            get
            {
                return int.MinValue;
            }
        }

        public void FormatLog(ref AddLogItemRequest logRequest)
        {
            var regex = new Regex("{rp#file#(.*)}");
            var match = regex.Match(logRequest.Text);
            if (match.Success)
            {
                logRequest.Text = logRequest.Text.Replace(match.Value, "");

                var filePath = match.Groups[1].Value;

                logRequest.Attach = new Client.Models.Attach(Path.GetFileName(filePath), MimeMapping.GetMimeMapping(filePath), File.ReadAllBytes(filePath));
            }
        }
    }
}
