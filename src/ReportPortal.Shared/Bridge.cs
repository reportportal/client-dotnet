using System;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ReportPortal.Shared
{
    public static class Bridge
    {
        static Bridge()
        {
            var currentDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            foreach (var file in currentDirectory.GetFiles("ReportPortal.Shared.*Extension.dll"))
            {
                AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(file.Name));
            }

            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetInterfaces().Contains(typeof(IBridgeExtension)))
                    {
                        var extension = Activator.CreateInstance(type);
                        Extensions.Add((IBridgeExtension)extension);
                    }
                }
            }
        }

        private static List<IBridgeExtension> Extensions = new List<IBridgeExtension>();

        public static Service Service { get; set; }

        private static ContextInfo _context;
        public static ContextInfo Context
        {
            get { return _context ?? (_context = new ContextInfo()); }
        }

        public static void LogMessage(LogLevel level, string message)
        {
            if (Extensions.Count != 0)
            {
                foreach(var extension in Extensions)
                {
                    extension.Log(level, message);
                }
            }

            if (Service != null && Context.LaunchId != null && Context.TestId != null)
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
