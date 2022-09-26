using System.Reflection;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Extentions
{
    static class EnumDescription
    {
        public static string GetDescriptionAttribute<T>(this T source)
        {
            var fi = source.GetType().GetRuntimeField(source.ToString());

            var attributes = (JsonPropertyNameAttribute[])fi.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false);

            return attributes.Length > 0 ? attributes[0].Name : source.ToString();
        }
    }
}
