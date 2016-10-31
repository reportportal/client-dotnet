using System;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Models;
using Newtonsoft.Json;

namespace ReportPortal.Client.Converters
{
    public class LogLevelConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(LogLevel) || objectType == typeof(LogLevel?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stringLogLevel = serializer.Deserialize<string>(reader);
            foreach (var enumValue in Enum.GetValues(typeof(LogLevel)))
            {
                if (enumValue.GetDescriptionAttribute() == stringLogLevel)
                {
                    return enumValue;
                }
            }
            return LogLevel.None;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var logLevel = (LogLevel)value;
            writer.WriteValue(logLevel.GetDescriptionAttribute());
        }
    }
}
