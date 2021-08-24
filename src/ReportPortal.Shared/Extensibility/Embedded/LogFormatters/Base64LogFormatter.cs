using System;
using System.Text.RegularExpressions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility.ReportEvents;

namespace ReportPortal.Shared.Extensibility.Embedded.LogFormatters
{
    /// <inheritdoc/>
    public class Base64LogFormatter : IReportEventsObserver
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
                var regex = new Regex("{rp#base64#(.*)#(.*)}");
                var match = regex.Match(logRequest.Text);
                if (match.Success)
                {
                    logRequest.Text = logRequest.Text.Replace(match.Value, "");

                    var mimeType = match.Groups[1].Value;
                    var bytes = Convert.FromBase64String(match.Groups[2].Value);

                    logRequest.Attach = new LogItemAttach(mimeType, bytes);
                }

            }
        }
    }
}
