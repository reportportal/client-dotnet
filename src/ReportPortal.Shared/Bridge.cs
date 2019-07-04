using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared.Extensibility;

namespace ReportPortal.Shared
{
    public static class Bridge
    {
        static Bridge()
        {
            LogFormatterExtensions = new List<ILogFormatter>();
            LogHandlerExtensions = new List<ILogHandler>();

            var currentDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            foreach (var file in currentDirectory.GetFiles("ReportPortal.*.dll"))
            {
                AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(file.Name));
            }

            var iLogFormatterExtensionInterfaceType = typeof(ILogFormatter);
            var iLogHandlerExtensionInterfaceType = typeof(ILogHandler);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name.StartsWith("ReportPortal")))
            {
                try
                {
                    foreach (var type in assembly.GetTypes().Where(t => t.IsClass))
                    {
                        if (iLogHandlerExtensionInterfaceType.IsAssignableFrom(type))
                        {
                            var extension = Activator.CreateInstance(type);
                            LogHandlerExtensions.Add((ILogHandler)extension);
                        }

                        if (iLogFormatterExtensionInterfaceType.IsAssignableFrom(type))
                        {
                            var extension = Activator.CreateInstance(type);
                            LogFormatterExtensions.Add((ILogFormatter)extension);
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {

                }
            }

            LogFormatterExtensions = LogFormatterExtensions.OrderBy(ext => ext.Order).ToList();
            LogHandlerExtensions = LogHandlerExtensions.OrderBy(ext => ext.Order).ToList();
        }


        public static List<ILogFormatter> LogFormatterExtensions { get; }

        public static List<ILogHandler> LogHandlerExtensions { get; }

        public static Service Service { get; set; }

        private static object _contextLockObj = new object();
        private static ContextInfo _context;
        public static ContextInfo Context
        {
            get
            {
                if (_context == null)
                {
                    lock (_contextLockObj)
                    {
                        _context = new ContextInfo();
                    }
                }

                return _context;
            }
        }

        /// <summary>
        /// Deprecated. Please use <see cref="Log"/> class to put your logs.
        /// </summary>
        [Obsolete("Please use static 'ReportPortal.Shared.Log' class to put your logs.")]
        public static void LogMessage(LogLevel level, string text)
        {
            var logRequest = new AddLogItemRequest
            {
                Level = level,
                Time = DateTime.UtcNow,
                Text = text
            };

            Log.Message(logRequest);
        }
    }
}
