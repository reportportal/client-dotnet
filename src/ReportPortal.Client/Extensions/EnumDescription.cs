using System.Reflection;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Extensions
{
    /// <summary>
    /// Provides extension methods for retrieving the description attribute of an enum value.
    /// </summary>
    static class EnumDescription
    {
        /// <summary>
        /// Retrieves the description attribute of the specified enum value.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="source">The enum value.</param>
        /// <returns>The description attribute value of the enum value.</returns>
        public static string GetDescriptionAttribute<T>(this T source)
        {
            var fieldInfo = source.GetType().GetRuntimeField(source.ToString());

            var attributes = (JsonPropertyNameAttribute[])fieldInfo.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false);

            return attributes.Length > 0 ? attributes[0].Name : source.ToString();
        }
    }
}