namespace ReportPortal.Client.Converters
{
    public class StringTrimmer
    {
        public static string Trim(string value, int maxSize)
        {
            if (value is string && (value).Length > maxSize)
            {
                value = (value).Substring(0, maxSize);
            }

            return value;
        }
    }
}
