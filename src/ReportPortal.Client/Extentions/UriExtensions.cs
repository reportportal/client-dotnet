using System;

namespace ReportPortal.Client.Extentions
{
    internal static class UriExtensions
    {
        public static Uri Normalize(this Uri uri)
        {
            var normalizedUriString = string.Format("{0}://{1}", uri.Scheme, uri.Authority);

            for (int i = 0; i < uri.Segments.Length; i++)
            {
                if (!uri.Segments[i].Equals("v1/", StringComparison.OrdinalIgnoreCase) && !uri.Segments[i].Equals("v1", StringComparison.OrdinalIgnoreCase))
                {
                    normalizedUriString += uri.Segments[i];
                }
            }

            normalizedUriString = normalizedUriString.TrimEnd("/".ToCharArray());

            if (!normalizedUriString.EndsWith("api", StringComparison.OrdinalIgnoreCase))
            {
                normalizedUriString += "/api";
            }

            if (!normalizedUriString.EndsWith("/"))
            {
                normalizedUriString += "/";
            }

            return new Uri(normalizedUriString);
        }
    }
}
