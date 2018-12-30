using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Ademund.Utils.XMLDocumentExtensions
{
    public sealed class StringWriterWithEncoding : StringWriter
	{
		public StringWriterWithEncoding(Encoding encoding)
		{
            Encoding = encoding;
		}

        public override Encoding Encoding { get; }
    }

	public static class XMLDocumentExtensions
	{
		public static string ToPrettyString(this XmlDocument doc, Encoding encoding)
		{
			if (doc == null)
				return null;

            var settings = new XmlWriterSettings {
                Encoding = encoding,// new UnicodeEncoding(false, false); // no BOM in a .NET string
                Indent = true,
                OmitXmlDeclaration = false
            };

            using (var stringWriter = new StringWriterWithEncoding(encoding))
            {
                //var stringWriter = new StringWriter(new StringBuilder());
                using (var xmlTextWriter = XmlWriter.Create(stringWriter, settings))
                {
                    doc.Save(xmlTextWriter);
                    return stringWriter.ToString();
                }
            }
		}

		public static XmlDocument ToXmlDocument(this XDocument xDocument)
		{
			var xmlDocument = new XmlDocument();
			using (var xmlReader = xDocument.CreateReader())
			{
				xmlDocument.Load(xmlReader);
			}
			return xmlDocument;
		}

		public static XDocument ToXDocument(this XmlDocument xmlDocument)
		{
			using (var nodeReader = new XmlNodeReader(xmlDocument))
			{
				nodeReader.MoveToContent();
				return XDocument.Load(nodeReader);
			}
		}
	}

	public static class XMLNodeExtensions
	{
		public static string ToPrettyString(this XmlNode node)
		{
			if (node == null)
				return null;

			string result = System.Xml.Linq.XElement.Parse(node.OuterXml).ToString();
			return result;
		}

		public static string GetUniqueXpath(this XmlNode node)
		{
			if (node.NodeType == XmlNodeType.Attribute)
			{
				// attributes have an OwnerElement, not a ParentNode; also they have             
				// to be matched by name, not found by position             
				return string.Format("{0}/@{1}", GetUniqueXpath(((XmlAttribute)node).OwnerElement), node.LocalName);
			}
			if (node.ParentNode == null)
			{
				// the only node with no parent is the root node, which has no path
				return "";
			}

			// check for a unique id or class attribute
			string[] attributesToLookFor = { "name", "id", "class" };
			foreach (string attributeToLookFor in attributesToLookFor)
			{
				if (node.Attributes[attributeToLookFor] != null)
				{
					string attributeValue = node.Attributes[attributeToLookFor].Value;
					if (!string.IsNullOrWhiteSpace(attributeValue))
					{
						string attributeXPath = string.Format("//*[local-name()='{0}' and @{1}='{2}']", node.LocalName, attributeToLookFor, attributeValue);
						var nodes = node.OwnerDocument.SelectNodes(attributeXPath);
						if (nodes.Count == 1)
							return attributeXPath;
					}
				}
			}

            if (node.SelectNodes(string.Format("following-sibling::*[local-name() = '{0}'] | preceding-sibling::*[local-name() = '{0}']", node.LocalName)).Count == 0)
                return string.Format("{0}/*[local-name()='{1}']", GetUniqueXpath(node.ParentNode), node.LocalName);

			//get the index
			int iIndex = 1;
			XmlNode xnIndex = node;
            while (xnIndex.PreviousSibling != null)
			{
				if (xnIndex.PreviousSibling.Name == node.Name)
					iIndex++;
				xnIndex = xnIndex.PreviousSibling;
			}

			// the path to a node is the path to its parent, plus "/node()[n]", where
			// n is its position among its siblings.         
			return string.Format("{0}/*[local-name()='{1}'][{2}]", GetUniqueXpath(node.ParentNode), node.LocalName, iIndex);
		}
	}
}