using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReportPortal.Shared.Extensibility
{
    public class ExtensionManager : IExtensionManager
    {
        private static readonly Internal.Logging.ITraceLogger _traceLogger = Internal.Logging.TraceLogManager.Instance.GetLogger(typeof(ExtensionManager));

        private static readonly Lazy<IExtensionManager> _instance = new Lazy<IExtensionManager>(() =>
            {
                var ext = new ExtensionManager();

                var assemblyLocation = AppContext.BaseDirectory;
                _traceLogger.Verbose($"Executing assembly location: {assemblyLocation}");

                ext.Explore(assemblyLocation);

                return ext;
            });

        public static IExtensionManager Instance => _instance.Value;

        private readonly List<string> _exploredPaths = new List<string>();

        private readonly List<string> _exploredAssemblies = new List<string>();

        private static readonly object _lockObj = new object();

        public void Explore(string path)
        {
            if (!_exploredPaths.Contains(path))
            {
                lock (_lockObj)
                {
                    if (!_exploredPaths.Contains(path))
                    {
                        var reportEventObservers = new List<IReportEventsObserver>();
                        var commandsListeners = new List<ICommandsListener>();

                        var currentDirectory = new DirectoryInfo(path);

                        _traceLogger.Info($"Exploring extensions in '{currentDirectory}' directory.");

                        foreach (var file in currentDirectory.GetFiles("*ReportPortal*.dll"))
                        {
                            _traceLogger.Verbose($"Found '{file.Name}' and loading it into current AppDomain.");
                            try
                            {
                                AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(file.Name));
                            }
                            catch (Exception ex)
                            {
                                _traceLogger.Warn($"Could not load extension assembly into application domain. {ex}");
                            }
                        }

                        var iReportEventObserseExtensionInterfaceType = typeof(IReportEventsObserver);
                        var iCommandsListenerInterfaceType = typeof(ICommandsListener);

                        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name.Contains("ReportPortal")))
                        {
                            if (!_exploredAssemblies.Contains(assembly.Location))
                            {
                                _exploredAssemblies.Add(assembly.Location);
                                _traceLogger.Verbose($"Exploring '{assembly.FullName}' assembly for extensions.");

                                try
                                {
                                    foreach (var type in assembly.GetTypes().Where(t => t.IsClass))
                                    {
                                        if (!type.IsAbstract && type.GetConstructors().Any(ctor => ctor.GetParameters().Length == 0))
                                        {
                                            if (iReportEventObserseExtensionInterfaceType.IsAssignableFrom(type))
                                            {
                                                var extension = Activator.CreateInstance(type);
                                                reportEventObservers.Add((IReportEventsObserver)extension);
                                                _traceLogger.Info($"Registered '{type.FullName}' type as {nameof(IReportEventsObserver)} extension.");
                                            }

                                            if (iCommandsListenerInterfaceType.IsAssignableFrom(type))
                                            {
                                                var extension = Activator.CreateInstance(type);
                                                commandsListeners.Add((ICommandsListener)extension);
                                                _traceLogger.Info($"Registered '{type.FullName}' type as {nameof(ICommandsListener)} extension.");
                                            }
                                        }
                                    }
                                }
                                catch (ReflectionTypeLoadException exp)
                                {
                                    _traceLogger.Warn($"Couldn't load '{assembly.GetName().Name}' assembly into domain.\n{exp}");
                                    foreach (var loaderException in exp.LoaderExceptions)
                                    {
                                        _traceLogger.Warn(loaderException.ToString());
                                    }
                                }
                            }
                        }

                        reportEventObservers.ForEach(reo => ReportEventObservers.Add(reo));
                        commandsListeners.ForEach(cl => CommandsListeners.Add(cl));

                        _exploredPaths.Add(path);
                    }
                }
            }
        }

        public IList<IReportEventsObserver> ReportEventObservers { get; } = new List<IReportEventsObserver>();

        public IList<ICommandsListener> CommandsListeners { get; } = new List<ICommandsListener>();
    }
}
