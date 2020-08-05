using System;
using System.Text.RegularExpressions;
using ReportPortal.Client.Abstractions.Requests;

namespace ReportPortal.Shared.Extensibility.LogFormatter
{
    /// <inheritdoc/>
    public class Base64LogFormatter : ILogFormatter
    {
        /// <inheritdoc/>
        public int Order => 10;

        /// <inheritdoc/>
        public bool FormatLog(CreateLogItemRequest logRequest)
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

                    logRequest.Attach = new LogItemAttach(mimeType, bytes);

                    return true;
                }

            }
            return false;
        }
    }
}
