using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Feast.JsonAnnotation.Structs.Doc
{
    internal class XmlNodeField
    {
        public required XmlElement Element { get; init; }
#nullable enable
        public XmlNodeField? Parent { get; init; }

        public string Tag => Element.Name;


        public string Name => Element.GetAttribute("name");

        public XmlElement CreateChildElement(XmlDocument document, string tag,string name)
        {
            var ret = document.CreateElement(tag);
            ret.SetAttribute("name", name);
            Element.AppendChild(ret);
            return ret;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Parent == null)
            {
                sb.Append(Tag);
            }
            else
            {
                sb.Append(Parent);
                sb.Append($"/{Tag}[@name=\"{Name}\"]");
            }
            return sb.ToString();
        }
    }
}
