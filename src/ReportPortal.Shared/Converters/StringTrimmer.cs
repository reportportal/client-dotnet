namespace ReportPortal.Shared.Converters
{
    internal static class StringTrimmer
    {
        public static string Trim(string value, int maxSize)
        {
            return (value != null && value.Length > maxSize) ? value.Substring(0, maxSize) : value;
        }
    }
}
