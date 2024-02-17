using ReportPortal.Client.Extensions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ReportPortal.Client.Abstractions.Filtering
{
    /// <summary>
    /// Represents a filter option for querying data.
    /// </summary>
    public class FilterOption
    {
        /// <summary>
        /// Gets or sets the paging options.
        /// </summary>
        public Paging Paging { get; set; }

        /// <summary>
        /// Gets or sets the sorting options.
        /// </summary>
        public Sorting Sorting { get; set; }

        /// <summary>
        /// Gets or sets the list of filters.
        /// </summary>
        public List<Filter> Filters { get; set; }

        /// <summary>
        /// Converts the filter option to a string representation.
        /// </summary>
        /// <returns>A string representation of the filter option.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            if (Paging != null)
            {
                builder.Append($"page.page={Paging.Number.ToString(CultureInfo.InvariantCulture)}");
                builder.Append($"&page.size={Paging.Size.ToString(CultureInfo.InvariantCulture)}");
            }
            if (Sorting != null)
            {
                builder.Append($"&page.sort={string.Join(",", Sorting.Fields.ToArray()) + "," + Sorting.Direction.GetDescriptionAttribute()}");
            }
            if (Filters != null)
            {
                foreach (var filter in Filters)
                {
                    var value = string.Join(",", filter.Values.Select(s => s.ToString()).ToArray());
                    builder.Append($"&filter.{filter.Operation.GetDescriptionAttribute()}.{filter.Field}={value}");
                }
            }

            return builder.ToString();
        }
    }
}
