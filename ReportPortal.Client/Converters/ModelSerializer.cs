using ReportPortal.Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ReportPortal.Client.Converters
{
    public class ModelSerializer
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
            catch (Exception exp)
            {
                throw new Exception($"Cannot deserialize json to {typeof(T).Name}.{Environment.NewLine}{json}", exp);
            }

            return result;
        }

        public static string Serialize<T>(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            var bytes = stream.ToArray();
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
