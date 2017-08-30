using System;

namespace ReportPortal.Client.Converters
{
    public class DateTimeConverter
    {
        public static DateTime ConvertTo(string dateString)
        {
            var doubleDate = double.Parse(dateString);
            return new DateTime(1970, 1, 1).AddMilliseconds(doubleDate);
        }

        public static string ConvertFrom(DateTime date)
        {
            return ((long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
        }
    }
}
