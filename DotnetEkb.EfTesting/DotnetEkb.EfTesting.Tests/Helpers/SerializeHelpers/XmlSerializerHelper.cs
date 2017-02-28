using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DotnetEkb.EfTesting.Tests.Helpers.SerializeHelpers
{
	public static class XmlSerializerHelper
	{
		public static string GetDefaultNamespace<T>()
		{
			return GetDefaultNamespace(typeof(T));
		}

		public static string GetDefaultNamespace(this Type type)
		{
			return type.GetCustomAttribute<XmlRootAttribute>()?.Namespace ?? string.Empty;
		}

		public static XmlSerializer GetXmlSerializer<T>()
		{
			return new XmlSerializer(typeof (T));
		}

		//public static T Deserialize<T>(this StringReader reader, IEventLogger logger)
		//{
		//    var serializer = new XmlSerializer(typeof (T));
		//    try
		//    {
		//        return (T) serializer.Deserialize(reader);
		//    }
		//    catch (Exception e)
		//    {
		//        logger.Write("Не удалось десериализовать объект " + e.StackTrace, EventTypes.Error, EventCategories.System, null);
		//        return default(T);
		//    }
		//}

		public static T Deserialize<T>(this Stream reader)
		{
			var serializer = new XmlSerializer(typeof (T));
			return (T) serializer.Deserialize(reader);
		}

		//public static T Deserialize<T>(this string xmlInString, IEventLogger logger)
		//{
		//    var serializer = new XmlSerializer(typeof (T));

		//    using (var reader = new StringReader(xmlInString))
		//    {
		//        try
		//        {
		//            return (T) serializer.Deserialize(reader);
		//        }
		//        catch (Exception e)
		//        {
		//            logger.Write("Не удалось десериализовать объект " + e.StackTrace, EventTypes.Error, EventCategories.System, null);
		//            return default(T);
		//        }
		//    }
		//}

		public static string SerializeUTF8(this XmlSerializer @this, object obj, bool needRemoveStandartNamespaces = false)
		{
			using (var memoryStream = new MemoryStream())
			{
				var xmlWriterSettings = new XmlWriterSettings
				{
					Encoding = new UTF8Encoding(true),
					Indent = true,
					OmitXmlDeclaration = false
				};
				var xmlTextWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
				if (needRemoveStandartNamespaces)
				{
					var xns = new XmlSerializerNamespaces();
					xns.Add("", "");
					@this.Serialize(xmlTextWriter, obj, xns);
				}
				else
				{
					@this.Serialize(xmlTextWriter, obj);
				}
				return Encoding.UTF8.GetString(memoryStream.ToArray());
			}
		}

		public static string SerializeUTF8<T>(T obj)
		{
			return new XmlSerializer(typeof (T)).SerializeUTF8(obj);
		}

		public static string Serialize(this XmlSerializer @this, object obj)
		{
			using (var stringWriter = new StringWriter())
			{
				@this.Serialize(stringWriter, obj);
				return stringWriter.ToString();
			}
		}

		public static byte[] SerializeBytes(this XmlSerializer @this, object obj)
		{
			var memoryStream = new MemoryStream();
			@this.Serialize(memoryStream, obj);
			return memoryStream.ToArray();
		}

		public static byte[] SerializeBytes<T>(T obj)
		{
			using (var memoryStream = new MemoryStream())
			{
				var xmlWriterSettings = new XmlWriterSettings
				{
					Encoding = new UTF8Encoding(true),
					Indent = true,
					OmitXmlDeclaration = false,
					CheckCharacters = false
				};
				var xmlTextWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
				var ser = new XmlSerializer(typeof (T));

				ser.Serialize(xmlTextWriter, obj);

				return memoryStream.ToArray();
			}
		}

		public static byte[] SerializeBytesUtf8<T>(T obj)
		{
			return SerializeBytes(obj);
		}

		public static object Deserialize(this XmlSerializer @this, string str)
		{
			return @this.Deserialize(new StringReader(str));
		}

		public static object DeserializeUTF8(this XmlSerializer @this, string str)
		{
			return @this.Deserialize(Encoding.UTF8.GetBytes(str));
		}

		public static object Deserialize1251(this XmlSerializer @this, string str)
		{
			var win1251 = Encoding.GetEncoding("windows-1251").GetBytes(str); 
			return @this.Deserialize(Encoding.UTF8.GetString(win1251));
		}

		public static T DeserializeUTF8<T>(string str)
		{
			var ser = new XmlSerializer(typeof (T));
			return (T) ser.DeserializeUTF8(str);
		}
		public static T Deserialize1251<T>(string str)
		{
			var ser = new XmlSerializer(typeof(T));
			return (T)ser.Deserialize1251(str);
		}

		public static T Deserialize<T>(string str)
		{
			var ser = new XmlSerializer(typeof (T));
			return (T) ser.Deserialize(str);
		}

		public static T Deserialize<T>(this byte[] bytes)
		{
			var ser = new XmlSerializer(typeof (T));
			var reader = XmlReader.Create(new MemoryStream(bytes), new XmlReaderSettings());
			return (T) ser.Deserialize(reader);
		}

		public static T Deserialize<T>(XmlReader reader)
		{
			var ser = new XmlSerializer(typeof (T));
			return (T) ser.Deserialize(reader);
		}

		public static string Serialize<T>(T obj, bool needRemoveStandartNamespaces = false)
		{
			var ser = new XmlSerializer(typeof (T));
			return ser.SerializeUTF8(obj, needRemoveStandartNamespaces);
		}

		public static object Deserialize(this XmlSerializer @this, byte[] bytes)
		{
			var memoryStream = new MemoryStream(bytes);
			return @this.Deserialize(memoryStream);
		}
	}
}