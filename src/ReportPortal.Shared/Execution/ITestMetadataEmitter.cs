using ReportPortal.Client.Abstractions.Models;
using System.Collections.Generic;

namespace ReportPortal.Shared.Execution
{
    /// <summary>
    /// Commands emitter to modify metadata of test on fly.
    /// </summary>
    public interface ITestMetadataEmitter
    {
        ICollection<ItemAttribute> Attributes { get; }
    }
}
