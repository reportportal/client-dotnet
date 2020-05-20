using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReportPortal.Shared.Extensibility
{
    public class ExtensionManager : IExtensionManager
    {
        private static Internal.Logging.ITraceLogger TraceLogger { get; } = Internal.Logging.TraceLogManager.Instance.GetLogger(typeof(ExtensionManager));

        private static Lazy<IExtensionManager> _instance = new Lazy<IExtensionManager>(() =>
            {
                var ext = new ExtensionManager();
                ext.Explore(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                ext.Explore(Environment.CurrentDirectory);
                return ext;
            });

        public static IExtensionManager Instance => _instance.Value;

        private List<string> _exploredPaths = new List<string>();

        private static object _lockObj = new object();

        public void Explore(string path)
        {
            if (!_exploredPaths.Contains(path))
            {
                lock (_lockObj)
                {
                    if (!_exploredPaths.Contains(path))
                    {
                        var logFormatters = new List<ILogFormatter>();
                        var logHandlers = new List<ILogHandler>();

                        var currentDirectory = new DirectoryInfo(path);

                        TraceLogger.Info($"Exploring extensions in '{currentDirectory}' directory.");

                        foreach (var file in currentDirectory.GetFiles("*ReportPortal*.dll"))
                        {
                            TraceLogger.Verbose($"Found '{file.Name}' and loading it into current AppDomain.");
                            AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(file.Name));
                        }

                        var iLogFormatterExtensionInterfaceType = typeof(ILogFormatter);
                        var iLogHandlerExtensionInterfaceType = typeof(ILogHandler);

                        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name.Contains("ReportPortal")))
                        {
                            TraceLogger.Verbose($"Exploring '{assembly.FullName}' assembly for extensions.");
                            try
                            {
                                foreach (var type in assembly.GetTypes().Where(t => t.IsClass))
                                {
                                    if (iLogHandlerExtensionInterfaceType.IsAssignableFrom(type))
                                    {
                                        var extension = Activator.CreateInstance(type);
                                        logHandlers.Add((ILogHandler)extension);
                                        TraceLogger.Info($"Registered '{type.FullName}' type as {nameof(ILogHandler)} extension.");
                                    }

                                    if (iLogFormatterExtensionInterfaceType.IsAssignableFrom(type))
                                    {
                                        var extension = Activator.CreateInstance(type);
                                        logFormatters.Add((ILogFormatter)extension);
                                        TraceLogger.Info($"Registered '{type.FullName}' type as {nameof(ILogFormatter)} extension.");
                                    }
                                }
                            }
                            catch (ReflectionTypeLoadException exp)
                            {
                                TraceLogger.Error($"Couldn't load '{assembly.GetName().Name}' assembly into domain. \n {exp}");
                                foreach (var loaderException in exp.LoaderExceptions)
                                {
                                    TraceLogger.Error(loaderException.ToString());
                                }
                            }
                        }

                        logFormatters.OrderBy(ext => ext.Order).ToList().ForEach(lf => LogFormatters.Add(lf));
                        logHandlers.OrderBy(ext => ext.Order).ToList().ForEach(lh => LogHandlers.Add(lh));

                        _exploredPaths.Add(path);
                    }
                }
            }
        }

        public IList<ILogFormatter> LogFormatters { get; } = new List<ILogFormatter>();

        public IList<ILogHandler> LogHandlers { get; } = new List<ILogHandler>();


    }
}
