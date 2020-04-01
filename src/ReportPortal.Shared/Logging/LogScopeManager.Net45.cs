#if NET45
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;

namespace ReportPortal.Shared.Logging
{

    sealed partial class LogScopeManager
    {
        private readonly string _logicalDataKey = "__AsyncLocalLogScope_Active__" + Guid.NewGuid().ToString("D");

        /// <summary>
        /// Returns current active LogScope which provides methods for logging.
        /// </summary>
        public ILogScope ActiveScope
        {
            get
            {
                var handle = CallContext.LogicalGetData(_logicalDataKey) as ObjectHandle;
                var activeScope = handle?.Unwrap() as ILogScope;

                if (activeScope == null)
                {
                    activeScope = RootScope;
                    ActiveScope = activeScope;
                }

                return activeScope;
            }
            set
            {
                CallContext.LogicalSetData(_logicalDataKey, new ObjectHandle(value));
            }
        }

        private static readonly object s_lockObj = new object();

        private ILogScope _rootScope;

        public ILogScope RootScope
        {
            get
            {
                if (_rootScope == null)
                {
                    lock (s_lockObj)
                    {
                        if (_rootScope == null)
                        {
                            _rootScope = new RootLogScope(this);
                        }
                    }
                }

                return _rootScope;
            }
            private set
            {
                _rootScope = value;
            }
        }
    }
}
#endif
