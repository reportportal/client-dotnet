using ReportPortal.Shared.Execution.Metadata;
using System;
using System.Collections.Generic;

namespace ReportPortal.Shared.Extensibility.Commands.CommandArgs
{
    /// <summary>
    /// Represents the arguments for the TestAttributesCommand event.
    /// </summary>
    public class TestAttributesCommandArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAttributesCommandArgs"/> class.
        /// </summary>
        /// <param name="attributes">The collection of meta attributes.</param>
        public TestAttributesCommandArgs(ICollection<MetaAttribute> attributes)
        {
            Attributes = attributes ?? new List<MetaAttribute>();
        }

        /// <summary>
        /// Gets the collection of meta attributes.
        /// </summary>
        public ICollection<MetaAttribute> Attributes { get; }
    }
}
