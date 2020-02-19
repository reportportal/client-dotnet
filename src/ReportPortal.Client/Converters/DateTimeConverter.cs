using System;

namespace ReportPortal.Client.Converters
{
    public class DateTimeConverter
    {
        private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ConvertTo(string dateString)
        {
            var doubleDate = double.Parse(dateString);
            return UNIX_EPOCH.AddMilliseconds(doubleDate);
        }

        public static string ConvertFrom(DateTime date)
        {
            return ((long)date.Subtract(UNIX_EPOCH).TotalMilliseconds).ToString();
        }
    }
}
