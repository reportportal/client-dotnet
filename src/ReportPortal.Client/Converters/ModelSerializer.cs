using System;
using System.IO;
using System.Runtime.Serialization;
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

            using (var stream = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(json);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;

                try
                {
                    return (T)serializer.ReadObject(stream);
                }
                catch (SerializationException exp)
                {
                    throw new SerializationException($"Cannot deserialize json to '{typeof(T).Name}' type.{Environment.NewLine}{json}", exp);
                }
            }
        }

        public static T Deserialize<T>(Stream stream)
        {
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true
            };

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), settings);

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            try
            {
                return (T)serializer.ReadObject(stream);
            }
            catch (SerializationException exp)
            {
                throw new SerializationException($"Cannot deserialize stream to '{typeof(T).Name}' type.", exp);
            }
        }

        public static string Serialize<T>(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                var bytes = stream.ToArray();
                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
        }

        public static void Serialize<T>(object obj, Stream stream)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            serializer.WriteObject(stream, obj);
        }
    }
}
