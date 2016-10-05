using System.ComponentModel;

namespace ReportPortal.Client.Extentions
{
    static class EnumDescription
    {
        public static string GetDescriptionAttribute<T>(this T source)
        {
            var fi = source.GetType().GetField(source.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : source.ToString();
        }
    }
}
