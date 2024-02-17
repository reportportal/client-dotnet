using System.Collections.Generic;

namespace ReportPortal.Shared.Execution.Metadata
{
    /// <summary>
    /// Represents a collection of meta attributes.
    /// </summary>
    public interface IMetaAttributesCollection : ICollection<MetaAttribute>
    {
        /// <summary>
        /// Adds a new meta attribute with the specified key and value to the collection.
        /// </summary>
        /// <param name="key">The key of the meta attribute.</param>
        /// <param name="value">The value of the meta attribute.</param>
        void Add(string key, string value);
    }
}
