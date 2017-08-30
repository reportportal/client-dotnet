using System;

namespace ReportPortal.Client.Converters
{
    public class DateTimeConverter
    {
        public static DateTime ConvertTo(string dateString)
        {
            var longDate = long.Parse(dateString);
            return new DateTime(1970, 1, 1).AddMilliseconds(longDate);
        }

        public static string ConvertFrom(DateTime date)
        {
            return (date - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString();
        }
    }
}
