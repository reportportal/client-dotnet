using ReportPortal.Client.Extentions;
using System;

namespace ReportPortal.Client.Converters
{
    public class EnumConverter
    {
        public static T ConvertTo<T>(string value)
        {
            T res = default(T);
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                if (enumValue.GetDescriptionAttribute() == value)
                {
                    res = enumValue;
                }
            }
            return res;
        }

        public static string ConvertFrom(Enum enumValue)
        {
            string enumString = null;

            if (enumValue != null)
            {
                enumString = enumValue.GetDescriptionAttribute();
            }

            return enumString;
        }
    }
}
