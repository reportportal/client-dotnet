using ReportPortal.Shared.Execution.Metadata;
using System.Collections.Generic;

namespace ReportPortal.Shared.Execution
{
    /// <summary>
    /// Commands emitter to modify metadata of test on fly.
    /// </summary>
    public interface ITestMetadataEmitter
    {
        IMetaAttributesCollection Attributes { get; }
    }
}
