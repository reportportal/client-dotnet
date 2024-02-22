using System;

namespace ReportPortal.Client.Extensions
{
    /// <summary>
    /// Provides extension methods for Uri class.
    /// </summary>
    internal static class UriExtensions
    {
        /// <summary>
        /// Normalizes the Uri by removing unnecessary segments and adding missing segments.
        /// </summary>
        /// <param name="uri">The Uri to normalize.</param>
        /// <returns>A normalized Uri.</returns>
        public static Uri Normalize(this Uri uri)
        {
            var normalizedUriString = $"{uri.Scheme}://{uri.Authority}";

            for (int i = 0; i < uri.Segments.Length; i++)
            {
                if (!uri.Segments[i].Equals("v1/", StringComparison.OrdinalIgnoreCase) &&
                    !uri.Segments[i].Equals("v1", StringComparison.OrdinalIgnoreCase))
                {
                    normalizedUriString += uri.Segments[i];
                }
            }

            normalizedUriString = normalizedUriString.TrimEnd('/');

            if (!normalizedUriString.EndsWith("api", StringComparison.OrdinalIgnoreCase))
            {
                normalizedUriString += "/api";
            }

            if (!normalizedUriString.EndsWith("/"))
            {
                normalizedUriString += '/';
            }

            return new Uri(normalizedUriString);
        }
    }
}
