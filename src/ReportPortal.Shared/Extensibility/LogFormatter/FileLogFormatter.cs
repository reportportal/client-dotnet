using ReportPortal.Client.Abstractions.Requests;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ReportPortal.Shared.Extensibility.LogFormatter
{
    /// <inheritdoc/>
    public class FileLogFormatter : ILogFormatter
    {
        /// <inheritdoc/>
        public int Order => 10;

        /// <inheritdoc/>
        public bool FormatLog(CreateLogItemRequest logRequest)
        {
            if (logRequest.Text != null)
            {
                var regex = new Regex("{rp#file#(.*)}");
                var match = regex.Match(logRequest.Text);
                if (match.Success)
                {
                    logRequest.Text = logRequest.Text.Replace(match.Value, "");

                    var filePath = match.Groups[1].Value;

                    try
                    {
                        var mimeType = MimeTypes.MimeTypeMap.GetMimeType(Path.GetExtension(filePath));

                        logRequest.Attach = new Client.Abstractions.Responses.Attach(mimeType, File.ReadAllBytes(filePath));

                        return true;
                    }
                    catch (Exception exp)
                    {
                        logRequest.Text += $"{Environment.NewLine}{Environment.NewLine}Cannot fetch data by `{filePath}` path.{Environment.NewLine}{exp}";
                    }
                }
            }
            return false;
        }
    }
}
