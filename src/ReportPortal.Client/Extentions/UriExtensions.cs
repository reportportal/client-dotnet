using System;

namespace ReportPortal.Client.Extentions
{
    public static class UriExtensions
    {
        public static Uri Normalize(this Uri uri)
        {
            var normalizedUri = uri;

            if (!uri.LocalPath.ToLowerInvariant().Contains("api/v1"))
            {
                normalizedUri = new Uri(uri + "api/v1/");
            }
            if (!normalizedUri.ToString().EndsWith("/"))
            {
                normalizedUri = new Uri(normalizedUri + "/");
            }

            return normalizedUri;
        }
    }
}
