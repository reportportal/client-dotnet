using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ReportPortal.Shared.Internal.Logging
{
    /// <summary>
    /// Class to manage all internal loggers.
    /// </summary>
    public static class TraceLogManager
    {
        static readonly SourceLevels _traceLevel;

        static TraceLogManager()
        {
            var envTraceLevelValue = Environment.GetEnvironmentVariable("ReportPortal_TraceLevel");

            if (!Enum.TryParse(envTraceLevelValue, out _traceLevel))
            {
                _traceLevel = SourceLevels.Error;
            }
        }

        readonly static object _lockObj = new object();

        static Dictionary<Type, ITraceLogger> _traceLoggers;

        /// <summary>
        /// Gets or creates new logger for requested type.
        /// </summary>
        /// <param name="type">Type where logger should be registered for</param>
        /// <returns><see cref="ITraceLogger"/> instance for logging internal messages</returns>
        public static ITraceLogger GetLogger(Type type)
        {
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
                    var traceSource = new TraceSource(type.Name);

                    traceSource.Switch = new SourceSwitch("ReportPortal_TraceSwitch", _traceLevel.ToString());

                    var logFileName = $"{type.Assembly.GetName().Name}.{Process.GetCurrentProcess().Id}.log";

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
        public static ITraceLogger GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }
    }
}
