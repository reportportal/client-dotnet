using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System;

namespace ReportPortal.Shared
{
    public static class Log
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

        public static void Info(string message, Attach attachment)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Info;
            logRequest.Attach = attachment;
            Message(logRequest);
        }

        public static void Debug(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Debug;
            Message(logRequest);
        }

        public static void Debug(string message, Attach attachment)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Debug;
            logRequest.Attach = attachment;
            Message(logRequest);
        }

        public static void Trace(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Trace;
            Message(logRequest);
        }

        public static void Trace(string message, Attach attachment)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Trace;
            logRequest.Attach = attachment;
            Message(logRequest);
        }

        public static void Error(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Error;
            Message(logRequest);
        }

        public static void Error(string message, Attach attachment)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Error;
            logRequest.Attach = attachment;
            Message(logRequest);
        }

        public static void Fatal(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Fatal;
            Message(logRequest);
        }

        public static void Fatal(string message, Attach attachment)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Fatal;
            logRequest.Attach = attachment;
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
