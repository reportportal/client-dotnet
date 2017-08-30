//using System;
//using ReportPortal.Client.Extentions;
//using ReportPortal.Client.Models;

//namespace ReportPortal.Client.Converters
//{
//    public class StatusConverter : JsonConverter
//    {
//        public override bool CanConvert(Type objectType)
//        {
//            return objectType == typeof(Status) || objectType == typeof(Status?);
//        }

//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//        {
//            var stringStatus = serializer.Deserialize<string>(reader);
//            foreach (var enumValue in Enum.GetValues(typeof(Status)))
//            {
//                if (enumValue.GetDescriptionAttribute() == stringStatus)
//                {
//                    return enumValue;
//                }
//            }
//            return Status.None;
//        }

//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            var status = (Status)value;
//            writer.WriteValue(status.GetDescriptionAttribute());
//        }
//    }
//}
