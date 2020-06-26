using ReportPortal.Shared.Execution;
using System;

namespace ReportPortal.Shared
{
    public static class Context
    {
        private static Lazy<ITestContext> _current = new Lazy<ITestContext>(() => new TestContext(Extensibility.ExtensionManager.Instance));

        public static ITestContext Current
        {
            get
            {
                return _current.Value;
            }
        }
    }
}
