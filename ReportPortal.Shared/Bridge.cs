using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared.Reporter;

namespace ReportPortal.Shared
{
    public static class Bridge
    {
        static Bridge()
        {
            Extensions = new List<IBridgeExtension>();

            var currentDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            foreach (var file in currentDirectory.GetFiles("ReportPortal.*.dll"))
            {
                AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(file.Name));
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
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
                catch (ReflectionTypeLoadException)
                {

                }
            }

            Extensions = Extensions.OrderBy(ext => ext.Order).ToList();
        }


        public static List<IBridgeExtension> Extensions { get; private set; }

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

            if (!handled && (Context.LaunchReporter as LaunchReporter)?.LastTestNode != null)
            {
                var testNode = Context.LaunchReporter.ChildTestReporters
                    .Select(t => FindNonFinishedTestReporter(t, Thread.CurrentThread.ManagedThreadId))
                    .FirstOrDefault(t => t != null) ?? (Context.LaunchReporter as LaunchReporter).LastTestNode;

                testNode.Log(request);
            }
        }

        private static ITestReporter FindNonFinishedTestReporter(ITestReporter testReporter, int threadId)
        {
            if (testReporter.FinishTask == null && !testReporter.ChildTestReporters.Any() && (testReporter as TestReporter).ThreadId == threadId)
            {
                return testReporter;
            }

            return testReporter.ChildTestReporters
                .Select(testNode => FindNonFinishedTestReporter(testNode, threadId))
                .FirstOrDefault(t => t != null);
        }
    }
}
