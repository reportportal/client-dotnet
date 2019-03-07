namespace ReportPortal.Client.Converter
{
    public static class StringTrimmer
    {
        public static string Trim(string value, int maxSize)
        {
            if (value != null && value.Length > maxSize)
            {
                value = value.Substring(0, maxSize);
            }

            return value;
        }
    }
}
