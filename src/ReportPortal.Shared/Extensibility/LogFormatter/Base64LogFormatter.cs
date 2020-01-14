using System;
using ReportPortal.Client.Requests;
using System.Text.RegularExpressions;

namespace ReportPortal.Shared.Extensibility.LogFormatter
{
    public class Base64LogFormatter : ILogFormatter
    {
        public int Order => 10;

        public bool FormatLog(ref AddLogItemRequest logRequest)
        {
            if (logRequest.Text != null)
            {
                var regex = new Regex("{rp#base64#(.*)#(.*)}");
                var match = regex.Match(logRequest.Text);
                if (match.Success)
                {
                    logRequest.Text = logRequest.Text.Replace(match.Value, "");

                    var mimeType = match.Groups[1].Value;
                    var bytes = Convert.FromBase64String(match.Groups[2].Value);

                    logRequest.Attach = new Client.Models.Attach("file", mimeType, bytes);

                    return true;
                }

            }
            return false;
        }
    }
}
