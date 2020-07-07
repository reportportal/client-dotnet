using ReportPortal.Shared.Execution.Metadata;

namespace ReportPortal.Shared.Execution
{
    /// <summary>
    /// Commands emitter to modify metadata of test on fly.
    /// </summary>
    public interface ITestMetadataEmitter
    {
        /// <summary>
        /// Collection of test meta attributes.
        /// </summary>
        /// <value>Returns a current collection of attributes.</value>
        IMetaAttributesCollection Attributes { get; }
    }
}
