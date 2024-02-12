using System.Collections.Generic;

namespace ReportPortal.Shared.Execution.Metadata
{
    public interface IMetaAttributesCollection : ICollection<MetaAttribute>
    {
        void Add(string key, string value);
    }
}
