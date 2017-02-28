using System.Xml;

namespace DotnetEkb.EfTesting.Tests.Helpers.ConvertHelpers
{
    public static class XmlDocumentConverter
    {
        public static string GetNodeInnerTxtOrNull(this XmlDocument xmlDocument, string path)
        {
            var node = xmlDocument.SelectSingleNode(path);
            return (node != null) ? node.InnerText : null;
        }
    }
}
