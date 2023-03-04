using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Feast.JsonAnnotation.Extensions;

namespace Feast.JsonAnnotation.Structs.Doc
{
    internal class XmlNodeField
    {
        public static XmlNodeField NewRootField(XmlDocument document) => new ()
        {
            Element = document.DocumentElement,
            Document = document
        };
        private XmlNodeField()
        {
            
        }
        public required XmlDocument Document { get; init; }
        public required XmlElement Element { get; init; }


#nullable enable
        public XmlNodeField? Parent { get; init; }

        public List<XmlNodeField> ChildNodes { get; init; } = new();

        public string Tag => Element.Name;

        public string Name => Element.GetAttribute("name");
        private XmlElement CreateChildElement(string tag,string name)
        {
            var ret = Document.CreateElement(tag);
            ret.SetAttribute("name", name);
            Element.AppendChild(ret);
            return ret;
        }
        public XmlNodeField CreateChildField(string tag, string name)
        {
            var ret = new XmlNodeField()
            {
                Parent = this,
                Element = CreateChildElement(tag, name),
                Document = Document,
            };
            ChildNodes.Add(ret);
            return ret;
        }

        /// <summary>
        /// <include file='' path='[@name=""]'/>
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string Annotation(string filePath)
        {
            return $"<include file='{filePath}' path='{ToCodePath()}'/>";
        }

        /// <summary>
        /// 返回获取节点的代码
        /// </summary>
        /// <param name="documentVariable"></param>
        /// <returns></returns>
        public string GetRouteCode(string documentVariable) =>
            Parent == null
                ? $"{documentVariable}.{nameof(XmlDocument.DocumentElement)}"
                : $"{documentVariable}.{nameof(XmlDocument.SelectSingleNode)}(\"{ToCodePath().Replace("\"","\\\"")}\")";

        /// <summary>
        /// 获取生成节点的代码
        /// </summary>
        /// <param name="documentVariable"></param>
        /// <returns></returns>
        public string GetGenerateCode(string documentVariable)
        {
            return Parent != null
                ? $@"
new {nameof(Action)}(() =>
{{
    if( {documentVariable}.{nameof(XmlDocument.SelectSingleNode)}(""{Parent.ReplaceForCode}"") == null ) throw new {nameof(ArgumentNullException)}(""Parent node not exist""); //Parent node not exist
    if( {documentVariable}.{nameof(XmlDocument.SelectSingleNode)}(""{ReplaceForCode}"") != null ) return; //Already generated
    var tmp = {documentVariable}.{nameof(XmlDocument.CreateElement)}(""{Tag}"");
    tmp.{nameof(XmlElement.SetAttribute)}(""name"", ""{Name}"");
    var code = {documentVariable}.{nameof(XmlDocument.CreateElement)}(""code"");
    tmp.{nameof(XmlNode.AppendChild)}(code);
    {documentVariable}.{nameof(XmlDocument.SelectSingleNode)}(""{Parent.ReplaceForCode}"")?.{nameof(XmlNode.AppendChild)}(tmp);
}}).{nameof(Action.Invoke)}();

{ ChildNodes.Select(x=>x.GetGenerateCode(documentVariable)).MultiLine() }
"
                : $@"
{documentVariable}.{nameof(XmlElement.AppendChild)}({documentVariable}.{nameof(Document.CreateElement)}(""{Tag}""));
{ChildNodes.Select(x => x.GetGenerateCode(documentVariable)).MultiLine()}
";
        }

        /// <summary>
        /// 生成注解路径(xpath)
        /// </summary>
        /// <returns></returns>
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

        public string ToCodePath() => $"{this}/code";

        public string ReplaceForCode => ToString().Replace("\"", "\\\"");
    }
}
