using ReportPortal.Shared.Execution.Metadata;
using System;
using System.Collections.Generic;

namespace ReportPortal.Shared.Extensibility.Commands.CommandArgs
{
    public class TestAttributesCommandArgs : EventArgs
    {
        public TestAttributesCommandArgs(ICollection<MetaAttribute> attributes)
        {
            Attributes = attributes ?? new List<MetaAttribute>();
        }

        public ICollection<MetaAttribute> Attributes { get; }
    }
}
