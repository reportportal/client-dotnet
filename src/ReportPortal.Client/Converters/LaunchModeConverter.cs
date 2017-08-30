//using System;
//using ReportPortal.Client.Extentions;
//using ReportPortal.Client.Models;

//namespace ReportPortal.Client.Converters
//{
//    public class LaunchModeConverter : JsonConverter
//    {
//        public override bool CanConvert(Type objectType)
//        {
//            return objectType == typeof(LaunchMode) || objectType == typeof(LaunchMode?);
//        }

//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//        {
//            var stringLogLevel = serializer.Deserialize<string>(reader);
//            foreach (var enumValue in Enum.GetValues(typeof(LaunchMode)))
//            {
//                if (enumValue.GetDescriptionAttribute() == stringLogLevel)
//                {
//                    return enumValue;
//                }
//            }
//            return LaunchMode.Default;
//        }

//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            var logLevel = (LaunchMode)value;
//            writer.WriteValue(logLevel.GetDescriptionAttribute());
//        }
//    }
//}
