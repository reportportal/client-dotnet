//using System;
//using ReportPortal.Client.Extentions;
//using ReportPortal.Client.Models;

//namespace ReportPortal.Client.Converters
//{
//    public class IssueTypeConverter
//    {
//        public bool CanConvert(Type objectType)
//        {
//            return objectType == typeof(IssueType) || objectType == typeof(IssueType?);
//        }

//        public object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//        {
//            var stringIssueType = serializer.Deserialize<string>(reader);
//            foreach (var enumValue in Enum.GetValues(typeof(IssueType)))
//            {
//                if (enumValue.GetDescriptionAttribute() == stringIssueType)
//                {
//                    return enumValue;
//                }
//            }
//            return IssueType.None;
//        }

//        public void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            var issueType = (IssueType)value;
//            writer.WriteValue(issueType.GetDescriptionAttribute());
//        }
//    }
//}
