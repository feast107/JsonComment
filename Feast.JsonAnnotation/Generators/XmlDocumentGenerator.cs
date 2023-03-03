using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Feast.JsonAnnotation.Filters;
using Feast.JsonAnnotation.Structs.Code;
using Feast.JsonAnnotation.Structs.Doc;
using XmlGenerateExtension = Feast.JsonAnnotation.Extensions.XmlGenerateExtension;

namespace Feast.JsonAnnotation.Generators
{
    internal class XmlDocumentGenerator
    {
        public XmlDocument Document { get; } = new();

        public XmlNodeConfig Config { get; init; } = new()
        {
            Root = nameof(Document),
            Class = "Class",
            Namespace = "Namespace"
        };

        public static XmlDocumentGenerator MapFile<TFilter>(FileRegion<TFilter> region) 
            where TFilter : SyntaxFilter<TFilter>
        {
            XmlDocumentGenerator ret = new();
            XmlGenerateExtension.Map(ret.Document,region,ret.Config);
            return ret;
        }
    }
}
