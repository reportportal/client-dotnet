using ReportPortal.Client.Requests;
using System.Collections.Generic;
using System.IO;
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

        private Dictionary<string, string> _mimeTypes = new Dictionary<string, string>();

        public FileLogFormatter()
        {
            _mimeTypes.Add("png", "image/png");
            _mimeTypes.Add("jpeg", "image/jpeg");
        }

        public void FormatLog(ref AddLogItemRequest logRequest)
        {
            var regex = new Regex("{rp#file#(.*)}");
            var match = regex.Match(logRequest.Text);
            if (match.Success)
            {
                logRequest.Text = logRequest.Text.Replace(match.Value, "");

                var filePath = match.Groups[1].Value;

                var fileExtension = Path.GetExtension(filePath);

                var mimeType = "application/octet-stream";
                _mimeTypes.TryGetValue(fileExtension, out mimeType);

                logRequest.Attach = new Client.Models.Attach(Path.GetFileName(filePath), mimeType, File.ReadAllBytes(filePath));
            }
        }
    }
}
