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
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            var bytes = Encoding.UTF8.GetBytes(json);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            return (T)serializer.ReadObject(stream);
        }

        public static string Serialize<T>(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            var bytes = stream.ToArray();
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
