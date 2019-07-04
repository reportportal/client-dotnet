using ReportPortal.Client.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Shared
{
    public class Log
    {
        public static void Message(AddLogItemRequest logRequest)
        {
            foreach (var handler in Bridge.LogHandlerExtensions)
            {
                var isHandled = handler.Handle(logRequest);

                if (isHandled) break;
            }
        }

        public static void Info(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Info;
            Message(logRequest);
        }

        public static void Debug(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Debug;
            Message(logRequest);
        }

        public static void Trace(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Trace;
            Message(logRequest);
        }

        public static void Error(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Error;
            Message(logRequest);
        }

        public static void Fatal(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Fatal;
            Message(logRequest);
        }

        private static AddLogItemRequest GetDefaultLogRequest(string text)
        {
            var logRequest = new AddLogItemRequest
            {
                Time = DateTime.UtcNow,
                Text = text
            };

            return logRequest;
        }
    }
}
