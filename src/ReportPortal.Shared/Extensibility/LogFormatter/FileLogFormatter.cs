using ReportPortal.Client.Abstractions.Requests;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReportPortal.Shared.Extensibility.LogFormatter
{
    public class FileLogFormatter : ILogFormatter
    {
        public int Order => 10;

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

                    if (!Path.GetInvalidPathChars().Any(i => filePath.Contains(i)))
                    {
                        var mimeType = MimeTypes.MimeTypeMap.GetMimeType(Path.GetExtension(filePath));

                        logRequest.Attach = new Client.Abstractions.Responses.Attach(Path.GetFileName(filePath), mimeType, File.ReadAllBytes(filePath));

                        return true;
                    }
                }
            }
            return false;
        }
    }
}
