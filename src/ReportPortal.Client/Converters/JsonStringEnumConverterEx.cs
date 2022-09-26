﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Converters
{
    internal class JsonStringEnumConverterEx<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
    {
        private readonly Dictionary<TEnum, string> _enumToString = new Dictionary<TEnum, string>();
        private readonly Dictionary<string, TEnum> _stringToEnum = new Dictionary<string, TEnum>();

        public JsonStringEnumConverterEx()
        {
            var type = typeof(TEnum);
            var values = Enum.GetValues(type);

            foreach (var value in values)
            {
                var enumMember = type.GetMember(value.ToString())[0];
                var attr = enumMember.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false)
                  .Cast<JsonPropertyNameAttribute>()
                  .FirstOrDefault();

                _stringToEnum.Add(value.ToString(), (TEnum)value);

                if (attr?.Name != null)
                {
                    _enumToString.Add((TEnum)value, attr.Name);
                    _stringToEnum.Add(attr.Name, (TEnum)value);
                }
                else
                {
                    _enumToString.Add((TEnum)value, value.ToString());
                }
            }
        }

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();

            if (_stringToEnum.TryGetValue(stringValue, out var enumValue))
            {
                return enumValue;
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(_enumToString[value]);
        }
    }
}
