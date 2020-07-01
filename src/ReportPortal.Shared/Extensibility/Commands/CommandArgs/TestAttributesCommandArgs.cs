using ReportPortal.Client.Abstractions.Models;
using System.Collections.Generic;

namespace ReportPortal.Shared.Extensibility.Commands.CommandArgs
{
    public class TestAttributesCommandArgs
    {
        public TestAttributesCommandArgs(ICollection<ItemAttribute> attributes)
        {
            Attributes = attributes ?? new List<ItemAttribute>();
        }

        public ICollection<ItemAttribute> Attributes { get; }
    }
}
