using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DotnetEkb.EfTesting.Tests.Helpers.XmlHelpers
{
    public static class XmlDocumentHelper
    {
        public static IDictionary<string, string> GetNamespaceDictionary(this XmlDocument xml)
        {
            var nameSpaceList = xml.SelectNodes(@"//namespace::*[not(. = ../../namespace::*)]").OfType<XmlNode>();
            return nameSpaceList.ToDictionary(xmlNode => xmlNode.LocalName, xmlNode => xmlNode.Value);
        }

        public static XmlNamespaceManager CreateNamespaceManager(this XmlDocument xml)
        {
            var manager = new XmlNamespaceManager(xml.NameTable);
            foreach (var name in xml.GetNamespaceDictionary())
            {
                manager.AddNamespace(name.Key, name.Value);
            }

            return manager;
        }

        public static XmlDocument LoadFromFile(string fileName)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            return xmlDocument;
        }

        public static XmlDocument LoadFromBytes(byte[] data)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(new MemoryStream(data));
            return xmlDocument;
        }

        public static XmlDocument LoadFromStream(Stream file)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(file);
            return xmlDocument;
        }

        public static XmlDocument LoadFromString(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            return xmlDocument;
        }

        public static XmlNode Rename(this XmlNode node, string name, string uri, string prefix)
        {
            if (node.NodeType == XmlNodeType.Element)
            {
                var newElement = node.OwnerDocument.CreateElement(prefix, name, uri);

                var oldElement = (XmlElement)node;
                while (oldElement.HasAttributes)
                {
                    newElement.SetAttributeNode(oldElement.RemoveAttributeNode(oldElement.Attributes[0]));
                }
                while (oldElement.HasChildNodes)
                {
                    newElement.AppendChild(oldElement.FirstChild);
                }
                if (oldElement.ParentNode != null)
                {
                    oldElement.ParentNode.ReplaceChild(newElement, oldElement);
                }

                return newElement;
            }

            return null;
        }

        public static XmlDocument SelectDocument(this XmlDocument xmlDocument, string xpath)
        {
            var resutlDocument = new XmlDocument();

            var childNode = xmlDocument.SelectSingleNode(xpath, xmlDocument.CreateNamespaceManager());
            var importNode = resutlDocument.ImportNode(childNode, true);
            resutlDocument.AppendChild(importNode);

            var xmlDeclaration = resutlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            var root = resutlDocument.DocumentElement;
            resutlDocument.InsertBefore(xmlDeclaration, root);

            foreach (var ns in xmlDocument.GetNamespaceDictionary())
            {
                root.SetAttribute($"xmlns:{ns.Key}", ns.Value);
            }

            return resutlDocument;

        }
    }
}
