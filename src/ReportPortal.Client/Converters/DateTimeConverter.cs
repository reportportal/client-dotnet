using System;
using System.Globalization;

namespace ReportPortal.Client.Converters
{
    public class DateTimeConverter
    {
        private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ConvertTo(string dateString)
        {
            try
            {
                var doubleDate = double.Parse(dateString);
                return UNIX_EPOCH.AddMilliseconds(doubleDate);
            }
            catch (FormatException)
            {
                return DateTime.Parse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            }

        }

        public static string ConvertFrom(DateTime date)
        {
            return ((long)date.Subtract(UNIX_EPOCH).TotalMilliseconds).ToString();
        }
    }
}
