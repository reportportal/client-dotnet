using System;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Models;
using Newtonsoft.Json;

namespace ReportPortal.Client.Converters
{
    public class TestItemTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TestItemType) || objectType == typeof(TestItemType?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stringItemType = serializer.Deserialize<string>(reader);
            foreach (var enumValue in Enum.GetValues(typeof(TestItemType)))
            {
                if (enumValue.GetDescriptionAttribute() == stringItemType)
                {
                    return enumValue;
                }
            }
            return TestItemType.None;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var itemType = (TestItemType)value;
            writer.WriteValue(itemType.GetDescriptionAttribute());
        }
    }
}
