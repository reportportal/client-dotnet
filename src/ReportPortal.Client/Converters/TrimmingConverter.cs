//using System;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace ReportPortal.Client.Converters
//{
//    public class TrimmingConverter : JsonConverter
//    {
//        private readonly int _maxSize;

//        public TrimmingConverter(int maxSize)
//        {
//            _maxSize = maxSize;
//        }

//        public override bool CanConvert(Type objectType)
//        {
//            return objectType == typeof(string);
//        }

//        public override bool CanRead { get { return false; } }

//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//        {
//            throw new NotImplementedException();
//        }

//        public override bool CanWrite { get { return true; } }

//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            if (value is string && ((string) value).Length > _maxSize)
//            {
//                value = ((string)value).Substring(0, _maxSize);
//            }

//            var t = JToken.FromObject(value);

//            t.WriteTo(writer);
//        }
//    }
//}
