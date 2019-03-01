using ReportPortal.Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ReportPortal.Client.Converters
{
    public static class ModelSerializer
    {
        public static T Deserialize<T>(string json)
        {
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true
            };
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), settings);
            MemoryStream stream = new MemoryStream();
            var bytes = Encoding.UTF8.GetBytes(json);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;

            T result;
            try
            {
                result = (T)serializer.ReadObject(stream);
            }
            catch (SerializationException exp)
            {
                throw new SerializationException($"Cannot deserialize json to '{typeof(T).Name}' type.{Environment.NewLine}{json}", exp);
            }

            return result;
        }

        public static string Serialize<T>(object o)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, o);
            var bytes = stream.ToArray();
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
