using System;
using Newtonsoft.Json;

namespace ReportPortal.Client.Converters
{
    public class DateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var longDate = serializer.Deserialize<long>(reader);
            return new DateTime(1970, 1, 1).AddMilliseconds(longDate);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = (DateTime)value;
            writer.WriteValue((long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds);
        }
    }
}
