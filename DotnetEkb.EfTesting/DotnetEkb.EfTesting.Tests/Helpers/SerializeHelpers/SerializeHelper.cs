using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DotnetEkb.EfTesting.Tests.Helpers.SerializeHelpers
{
    public class SerializeHelper
    {
        public static byte[] XmlSerialize<T>(T obj)
        {
            return XmlSerialize(obj, "");
        }

        public static byte[] XmlSerialize<T>(T obj, string serializeNamespace)
        {
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false),
                OmitXmlDeclaration = true
            };
            var ns = new XmlSerializerNamespaces();
            if (serializeNamespace != null)
            {
                ns.Add("", serializeNamespace);
            }

            var xmlSerializer = new XmlSerializer(obj.GetType());
            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, settings))
                {
                    xmlSerializer.Serialize(xmlWriter, obj, ns);
                    return stream.ToArray();
                }
            }
        }

        public static Stream SerializeToStream<T>(T obj)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream;
        }

        public static T XmlDeserialize<T>(byte[] bytes)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream(bytes))
            {
                return (T)xmlSerializer.Deserialize(stream);
            }
        }

        public static object XmlDeserialize(Type type, byte[] bytes)
        {
            var xmlSerializer = new XmlSerializer(type);
            using (var stream = new MemoryStream(bytes))
            {
                return xmlSerializer.Deserialize(stream);
            }
        }

        public static object XmlDeserialize<T>(Type type, byte[] bytes)
        {
            return (T)XmlDeserialize(type, bytes);
        }

        public static string XmlBase64Serialize<T>(T data)
        {
            return Convert.ToBase64String(XmlSerialize(data));
        }

        public static T XmlBase64Deserialize<T>(string base64String)
        {
            return XmlDeserialize<T>(Convert.FromBase64String(base64String));
        }

        public static byte[] BinarySerialize(object data)
        {
            var bf = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                bf.Serialize(stream, data);
                return stream.ToArray();
            }
        }

        public static T BinaryDeserialize<T>(byte[] data)
        {
            var bf = new BinaryFormatter();
            using (var stream = new MemoryStream(data))
            {
                return (T) bf.Deserialize(stream);
            }
        }
    }
}