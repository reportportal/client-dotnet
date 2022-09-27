using System;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ReportPortal.Client.Converters
{
    sealed class DateTimeUnixEpochConverter : JsonConverter<DateTime>
    {
        private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var unixTime = reader.GetInt64();

            return UNIX_EPOCH.AddMilliseconds(unixTime);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var unixTime = Convert.ToInt64((value - UNIX_EPOCH).TotalMilliseconds);

            writer.WriteNumberValue(unixTime);
        }
    }

    sealed class NullableDateTimeUnixEpochConverter : JsonConverter<DateTime?>
    {
        private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var unixTime = reader.GetInt64();

            return UNIX_EPOCH.AddMilliseconds(unixTime);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            var unixTime = Convert.ToInt64((value - UNIX_EPOCH).Value.TotalMilliseconds);

            writer.WriteNumberValue(unixTime);
        }
    }
}
