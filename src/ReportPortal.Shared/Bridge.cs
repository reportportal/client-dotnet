using System;
using ReportPortal.Client;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace ReportPortal.Shared
{
    public static class Bridge
    {
        static Bridge()
        {
            var currentDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            foreach (var file in currentDirectory.GetFiles("ReportPortal.*.dll"))
            {
                AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(file.Name));
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
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

            Extensions = Extensions.OrderBy(ext => ext.Order).ToList();
        }

        private static readonly List<IBridgeExtension> Extensions = new List<IBridgeExtension>();

        public static Service Service { get; set; }

        private static ContextInfo _context;
        public static ContextInfo Context => _context ?? (_context = new ContextInfo());

        public static void LogMessage(LogLevel level, string text)
        {
            var request = new AddLogItemRequest
            {
                Level = level,
                Time = DateTime.UtcNow,
                Text = text
            };

            var handled = false;

            foreach (var extension in Extensions)
            {
                extension.FormatLog(ref request);
                if (extension.Handled)
                {
                    handled = true;
                    break;
                }
            }

            if (!handled && Context.LaunchReporter != null)
            {
                Context.LaunchReporter.LastTestNode.Log(request);
            }
        }
    }
}
