using ReportPortal.Client.Requests;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

                if (!Path.GetInvalidPathChars().Any(i => filePath.Contains(i)))
                {
                    var mimeType = MimeTypes.MimeTypeMap.GetMimeType(Path.GetExtension(filePath));

                    logRequest.Attach = new Client.Models.Attach(Path.GetFileName(filePath), mimeType, File.ReadAllBytes(filePath));
                }
            }
        }
    }
}
