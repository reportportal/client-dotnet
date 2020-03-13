using ReportPortal.Client.Extentions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ReportPortal.Client.Abstractions.Filtering
{
    public class FilterOption
    {
        public Paging Paging { get; set; }

        public Sorting Sorting { get; set; }

        public List<Filter> Filters { get; set; }

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
