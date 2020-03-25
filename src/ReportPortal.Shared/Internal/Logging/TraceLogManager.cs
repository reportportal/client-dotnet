using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ReportPortal.Shared.Internal.Logging
{
    /// <summary>
    /// Class to manage all internal loggers.
    /// </summary>
    public class TraceLogManager
    {
        static TraceLogManager()
        {

        }

        private static readonly Lazy<TraceLogManager> _instance = new Lazy<TraceLogManager>(() => new TraceLogManager());

        public static TraceLogManager Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private string _baseDir = Environment.CurrentDirectory;


        /// <summary>
        /// Fluently sets BaseDir.
        /// </summary>
        /// <param name="baseDir"></param>
        /// <returns></returns>
        public TraceLogManager WithBaseDir(string baseDir)
        {
            _baseDir = baseDir;

            return this;
        }

        readonly static object _lockObj = new object();

        static Dictionary<Type, ITraceLogger> _traceLoggers;

        /// <summary>
        /// Gets or creates new logger for requested type.
        /// </summary>
        /// <param name="type">Type where logger should be registered for</param>
        /// <returns><see cref="ITraceLogger"/> instance for logging internal messages</returns>
        public ITraceLogger GetLogger(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (_traceLoggers == null)
            {
                lock (_lockObj)
                {
                    if (_traceLoggers == null)
                    {
                        _traceLoggers = new Dictionary<Type, ITraceLogger>();
                    }
                }
            }

            lock (_lockObj)
            {
                if (!_traceLoggers.ContainsKey(type))
                {
                    var envTraceLevelValue = Environment.GetEnvironmentVariable("ReportPortal_TraceLevel");

                    SourceLevels traceLevel;

                    if (!Enum.TryParse(envTraceLevelValue, out traceLevel))
                    {
                        traceLevel = SourceLevels.Error;
                    }

                    var traceSource = new TraceSource(type.Name);

                    traceSource.Switch = new SourceSwitch("ReportPortal_TraceSwitch", traceLevel.ToString());

                    var logFileName = $"{type.Assembly.GetName().Name}.{Process.GetCurrentProcess().Id}.log";

                    logFileName = Path.Combine(_baseDir, logFileName);

                    var traceListener = new DefaultTraceListener
                    {
                        Filter = new SourceFilter(traceSource.Name),
                        LogFileName = logFileName
                    };

                    traceSource.Listeners.Add(traceListener);

                    _traceLoggers[type] = new TraceLogger(traceSource);
                }
            }

            return _traceLoggers[type];
        }

        /// <summary>
        /// Gets or creates new logger for requested type.
        /// </summary>
        /// <typeparam name="T">Type where logger should be registered for</typeparam>
        /// <returns><see cref="ITraceLogger"/> instance for logging internal messages</returns>
        public ITraceLogger GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }
    }
}
