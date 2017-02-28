using System.Xml;

namespace DotnetEkb.EfTesting.Tests.Helpers.XmlHelpers
{
    public static class XmlNodeHelper
    {
        public static void Replace(this XmlNode destinationXmlElement, string destinationXpath, XmlNode sourceXmlElement, string sourceXpath)
        {
            var namespaceManager = destinationXmlElement.OwnerDocument.CreateNamespaceManager();

            var sourceNode = sourceXmlElement.SelectSingleNode(sourceXpath, namespaceManager);
            if (sourceNode != null)
            {
                var importNode = destinationXmlElement.OwnerDocument.ImportNode(sourceNode, true);
                var oldNode = destinationXmlElement.SelectSingleNode(destinationXpath, namespaceManager);
                destinationXmlElement.ReplaceChild(importNode, oldNode);
            }
        }

        public static XmlElement GetOrCreateChildElement(this XmlNode xmlNode, string childName, string childNamespace)
        {
            var namespaceManager = xmlNode.OwnerDocument.CreateNamespaceManager();

            var childElement = xmlNode.SelectSingleNode(childName, namespaceManager) as XmlElement;
            if (childElement == null)
            {
                childElement = xmlNode.OwnerDocument.CreateElement(childName, childNamespace);
                xmlNode.AppendChild(childElement);
            }

            return childElement;
        }
    }
}
