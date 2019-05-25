using System;
using ReportPortal.Client.Requests;
using System.Text.RegularExpressions;

namespace ReportPortal.Shared.LogFormatters
{
    public class Base64LogFormatter : IBridgeExtension
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
            var regex = new Regex("{rp#base64#(.*)#(.*)}");
            var match = regex.Match(logRequest.Text);
            if (match.Success)
            {
                logRequest.Text = logRequest.Text.Replace(match.Value, "");

                var mimeType = match.Groups[1].Value;
                var bytes = Convert.FromBase64String(match.Groups[2].Value);

                logRequest.Attach = new Client.Models.Attach("file", mimeType, bytes);
            }
        }
    }
}
