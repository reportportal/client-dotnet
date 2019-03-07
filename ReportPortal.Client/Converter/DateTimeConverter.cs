using System;
using System.Globalization;

namespace ReportPortal.Client.Converter
{
    public static class DateTimeConverter
    {
        public static DateTime ConvertTo(string dateString)
        {
            var doubleDate = double.Parse(dateString, CultureInfo.InvariantCulture);
            return new DateTime(1970, 1, 1).AddMilliseconds(doubleDate);
        }

        public static string ConvertFrom(DateTime date)
        {
            return ((long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(CultureInfo.InvariantCulture);
        }
    }
}
