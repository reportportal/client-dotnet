using ReportPortal.Client.Abstractions.Models;
using System;

namespace ReportPortal.Shared.Converters
{
    /// <summary>
    /// Converter of any string to <see cref="ItemAttribute"/> instance.
    /// </summary>
    public class ItemAttributeConverter
    {
        /// <summary>
        /// Translate string to ItemAttribute
        /// 
        /// component:search =>     key=component, value=search
        /// :search                 key=, value=search
        /// search:                 key=, value=search
        /// 
        /// Attribute value always should not be empty.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="optionsProvider"></param>
        /// <returns></returns>
        public ItemAttribute ConvertFrom(string tag, Action<ConvertOptions> optionsProvider = null)
        {
            var options = ConvertOptions.Default;

            if (optionsProvider != null)
            {
                optionsProvider(options);
            }

            var attr = new ItemAttribute();

            var values = tag.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 1 || string.IsNullOrEmpty(values[1]))
            {
                attr.Key = options.UndefinedKey;
                attr.Value = values[0];
            }
            else
            {
                attr.Key = values[0];
                attr.Value = tag.Substring(values[0].Length + 1);
            }

            return attr;
        }

        /// <summary>
        /// Defines options for <see cref="ItemAttributeConverter"/>.
        /// </summary>
        public class ConvertOptions
        {
            /// <summary>
            /// Key if it was not parsed.
            /// </summary>
            public string UndefinedKey { get; set; }

            /// <summary>
            /// Returns default converter options.
            /// </summary>
            public static ConvertOptions Default
            {
                get
                {
                    return new ConvertOptions();
                }
            }
        }
    }
}
