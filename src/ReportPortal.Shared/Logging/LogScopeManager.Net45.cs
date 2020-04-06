#if NET45
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;

namespace ReportPortal.Shared.Logging
{

    sealed partial class LogScopeManager
    {
        private readonly string _logicalActiveDataKey = "__AsyncLocalLogScope_Active__" + Guid.NewGuid().ToString("D");

        private readonly string _logicalRootDataKey = "__AsyncLocalLogScope_Root__" + Guid.NewGuid().ToString("D");

        /// <summary>
        /// Returns current active LogScope which provides methods for logging.
        /// </summary>
        public ILogScope ActiveScope
        {
            get
            {
                var handle = CallContext.LogicalGetData(_logicalActiveDataKey) as ObjectHandle;
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
                CallContext.LogicalSetData(_logicalActiveDataKey, new ObjectHandle(value));
            }
        }

        private static readonly object s_lockObj = new object();

        public ILogScope RootScope
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
                        rootScope = new RootLogScope(this);

                        RootScope = rootScope;
                    }
                }

                return rootScope;
            }
            private set
            {
                CallContext.LogicalSetData(_logicalRootDataKey, new ObjectHandle(value));
            }
        }
    }
}
#endif
