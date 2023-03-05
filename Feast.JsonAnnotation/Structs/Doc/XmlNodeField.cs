using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Feast.JsonAnnotation.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonAnnotation.Structs.Doc
{
    internal class XmlNodeField
    {
        public static XmlNodeField NewRootField(XmlDocument document) => new ()
        {
            Element = document.DocumentElement,
            Document = document
        };
        private XmlNodeField() { }
        public required XmlDocument Document { get; init; }
        public required XmlElement Element { get; init; }
#nullable enable
        public XmlNodeField? Parent { get; init; }

        public List<XmlNodeField> ChildNodes { get; init; } = new();

        public string Tag => Element.Name;

        public string Name => Element.GetAttribute("name");

        private string? cache;
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

        public string SetJsonProvider(string jsonGenerateCode) =>
            cache = GetGenerateCode().Replace("[provider]", jsonGenerateCode)
                    ?? throw new NullReferenceException("Code hadn't been generate");

        public string GetGenerateCode(bool generateRoot = true)
        {
            return cache ??= Parent != null
                ? $@"
new {nameof(Action)}<{nameof(String)}>((codeString) =>
{{
    if( [document].{nameof(XmlDocument.SelectSingleNode)}(""{Parent.ReplaceForCode}"") == null ) throw new {nameof(ArgumentNullException)}(""Parent node not exist""); //Parent node not exist
    if( [document].{nameof(XmlDocument.SelectSingleNode)}(""{ReplaceForCode}"") != null ) return; //Already generated
    var tmp = [document].{nameof(XmlDocument.CreateElement)}(""{Tag}"");
    tmp.{nameof(XmlElement.SetAttribute)}(""name"", ""{Name}"");
    var code = [document].{nameof(XmlDocument.CreateElement)}(""code"");
    code.{nameof(XmlNode.InnerText)} = codeString;
    tmp.{nameof(XmlNode.AppendChild)}(code);
    [document].{nameof(XmlDocument.SelectSingleNode)}(""{Parent.ReplaceForCode}"")?.{nameof(XmlNode.AppendChild)}(tmp);
}}).{nameof(Action.Invoke)}([provider]);"
                : generateRoot
                    ? $@"[document].{nameof(XmlElement.AppendChild)}([document].{nameof(Document.CreateElement)}(""{Tag}""));"
                    : string.Empty;
        }

        /// <summary>
        /// 获取生成节点的代码
        /// </summary>
        /// <param name="documentVariable"></param>
        /// <param name="generateRoot">是否需要生成根节点</param>
        /// <returns></returns>
        public string GetGenerateCode(string documentVariable,bool generateRoot = true)
        {
            return ChildNodes.Count == 0
                ? string.Empty
                : $@"{GetGenerateCode(generateRoot).Replace("[document]", documentVariable).Replace("[provider]", "\"\"")}
{ChildNodes.Select(x => x.GetGenerateCode(documentVariable, generateRoot)).MultiLine()}";
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
