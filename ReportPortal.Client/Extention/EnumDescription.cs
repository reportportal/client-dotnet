using System.Reflection;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Extention
{
    static class EnumDescription
    {
        public static string GetDescriptionAttribute<T>(this T source)
        {
            var fi = source.GetType().GetRuntimeField(source.ToString());

            var attributes = (DataMemberAttribute[])fi.GetCustomAttributes(typeof(DataMemberAttribute), false);

            return attributes.Length > 0 ? attributes[0].Name : source.ToString();
        }
    }
}
