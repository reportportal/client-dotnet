using System;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace ReportPortal.Shared
{
    public static class Bridge
    {
        public static Service Service { get; set; }

        private static ContextInfo _context;
        public static ContextInfo Context
        {
            get { return _context ?? (_context = new ContextInfo()); }
        }

        public static void LogMessage(LogLevel level, string message)
        {
            if (Service != null && Context.LaunchId != null)
            {
                var request = new AddLogItemRequest
                {
                    TestItemId = Context.TestId,
                    Level = level,
                    Time = DateTime.UtcNow,
                    Text = message
                };

                Service.AddLogItemAsync(request);
            }
        }
    }
}
