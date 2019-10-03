using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System;
using System.IO;

namespace ReportPortal.Shared
{
    public static class Log
    {
        private const string FileName = "attachment_name";

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

        public static void Info(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Info;
            logRequest.Attach = GetAttachFromFileInfo(mimeType, content);
            Message(logRequest);
        }

        public static void Debug(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Debug;
            Message(logRequest);
        }

        public static void Debug(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Debug;
            logRequest.Attach = GetAttachFromFileInfo(mimeType, content);
            Message(logRequest);
        }

        public static void Trace(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Trace;
            Message(logRequest);
        }

        public static void Trace(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Trace;
            logRequest.Attach = GetAttachFromFileInfo(mimeType, content);
            Message(logRequest);
        }

        public static void Error(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Error;
            Message(logRequest);
        }

        public static void Error(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Error;
            logRequest.Attach = GetAttachFromFileInfo(mimeType, content);
            Message(logRequest);
        }

        public static void Fatal(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Fatal;
            Message(logRequest);
        }

        public static void Fatal(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = Client.Models.LogLevel.Fatal;
            logRequest.Attach = GetAttachFromFileInfo(mimeType, content);
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

        private static Attach GetAttachFromFileInfo(string mimeType, byte[] content)
        {
            return new Attach(FileName, mimeType, content);
        }
    }
}
