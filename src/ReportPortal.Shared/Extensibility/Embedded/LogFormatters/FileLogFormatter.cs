using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility.ReportEvents;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ReportPortal.Shared.Extensibility.Embedded.LogFormatters
{
    /// <inheritdoc/>
    public class FileLogFormatter : IReportEventsObserver
    {
        /// <inheritdoc/>
        public void Initialize(IReportEventsSource reportEventsSource)
        {
            reportEventsSource.OnBeforeLogsSending += ReportEventsSource_OnBeforeLogsSending;
        }

        private void ReportEventsSource_OnBeforeLogsSending(Reporter.ILogsReporter logsReporter, ReportEvents.EventArgs.BeforeLogsSendingEventArgs args)
        {
            if (args.CreateLogItemRequests != null)
            {
                foreach (var logRequest in args.CreateLogItemRequests)
                {
                    FormatLog(logRequest);
                }
            }
        }

        /// <inheritdoc/>
        private void FormatLog(CreateLogItemRequest logRequest)
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

                        logRequest.Attach = new LogItemAttach(mimeType, File.ReadAllBytes(filePath));
                    }
                    catch (Exception exp)
                    {
                        logRequest.Text += $"{Environment.NewLine}{Environment.NewLine}Cannot fetch data by `{filePath}` path.{Environment.NewLine}{exp}";
                    }
                }
            }
        }
    }
}
