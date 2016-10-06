using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ReportPortal.Client.Extentions;

namespace ReportPortal.Client.Filtering
{
    public class FilterOption
    {
        public Paging Paging { get; set; }

        public Sorting Sorting { get; set; }

        public List<Filter> Filters { get; set; }

        public Dictionary<string, string> ConvertToDictionary()
        {
            var parameters = new Dictionary<string, string>();
            if (Paging != null)
            {
                parameters["page.page"] = Paging.Number.ToString(CultureInfo.InvariantCulture);
                parameters["page.size"] = Paging.Size.ToString(CultureInfo.InvariantCulture);
            }
            if (Sorting != null)
            {
                parameters["page.sort"] = string.Join(",", Sorting.Fields.ToArray()) + "," +
                                          Sorting.Direction.GetDescriptionAttribute();
            }
            if (Filters != null)
            {
                foreach (var filter in Filters)
                {
                    var value = string.Join(",", filter.Values.Select(s => s.ToString()).ToArray());
                    parameters["filter." + filter.Operation.GetDescriptionAttribute() + "." + filter.Field] = value;
                }
            }
            return parameters;
        }
    }
}
