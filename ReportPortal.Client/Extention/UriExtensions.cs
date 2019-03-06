using System;
using System.Globalization;
using System.Linq;

namespace ReportPortal.Client.Extention
{
    public static class UriExtensions
    {
        public static Uri Append(this Uri uri, params string[] paths)
        {
            return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format(CultureInfo.InvariantCulture, "{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
        }
    }
}
