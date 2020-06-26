using ReportPortal.Shared.Execution.Log;
using ReportPortal.Shared.Extensibility;
using System;
using System.Collections.Generic;
#if NET45
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
#endif
using System.Text;
using System.Threading;

namespace ReportPortal.Shared.Execution
{
    class TestContext : ITestContext
    {
        private IExtensionManager _extensionManager;

        public TestContext(IExtensionManager extensionManager)
        {
            _extensionManager = extensionManager;
        }

#if !NET45
        private AsyncLocal<ILogScope> _activeLogScope = new AsyncLocal<ILogScope>();

        private AsyncLocal<ILogScope> _rootLogScope = new AsyncLocal<ILogScope>();

        /// <summary>
        /// Returns current active LogScope which provides methods for logging.
        /// </summary>
        public ILogScope Log
        {
            get
            {
                if (_activeLogScope.Value == null)
                {
                    Log = RootScope;
                }

                return _activeLogScope.Value;
            }
            set
            {
                _activeLogScope.Value = value;
            }
        }

        private ILogScope RootScope
        {
            get
            {
                if (_rootLogScope.Value == null)
                {
                    //TraceLogger.Info($"New log context identified, activating {typeof(RootLogScope).Name}");
                    RootScope = new RootLogScope(this, _extensionManager);
                }

                return _rootLogScope.Value;
            }
            set
            {
                _rootLogScope.Value = value;
            }
        }
#else
        private readonly string _logicalActiveDataKey = "__AsyncLocalLogScope_Active__" + Guid.NewGuid().ToString("D");

        private readonly string _logicalRootDataKey = "__AsyncLocalLogScope_Root__" + Guid.NewGuid().ToString("D");

        /// <summary>
        /// Returns current active LogScope which provides methods for logging.
        /// </summary>
        public ILogScope Log
        {
            get
            {
                var handle = CallContext.LogicalGetData(_logicalActiveDataKey) as ObjectHandle;
                var activeScope = handle?.Unwrap() as ILogScope;

                if (activeScope == null)
                {
                    activeScope = RootScope;
                    
                    Log = activeScope;
                }

                return activeScope;
            }
            set
            {
                CallContext.LogicalSetData(_logicalActiveDataKey, new ObjectHandle(value));
            }
        }

        private static readonly object s_lockObj = new object();

        private ILogScope RootScope
        {
            get
            {
                ILogScope rootScope;

                lock (s_lockObj)
                {
                    var handle = CallContext.LogicalGetData(_logicalRootDataKey) as ObjectHandle;
                    rootScope = handle?.Unwrap() as ILogScope;

                    if (rootScope == null)
                    {
                        rootScope = new RootLogScope(this, _extensionManager);

                        RootScope = rootScope;
                    }
                }

                return rootScope;
            }
            set
            {
                CallContext.LogicalSetData(_logicalRootDataKey, new ObjectHandle(value));
            }
        }
#endif
    }
}
