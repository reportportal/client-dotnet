using System;

namespace ReportPortal.Client.Converter
{
    public static class DateTimeConverter
    {
        public static DateTime ConvertTo(string dateString)
        {
            double doubleDate = double.Parse(dateString, System.Globalization.CultureInfo.InvariantCulture);
            return new DateTime(1970, 1, 1).AddMilliseconds(doubleDate);
        }

        public static string ConvertFrom(DateTime date)
        {
            return ((long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
