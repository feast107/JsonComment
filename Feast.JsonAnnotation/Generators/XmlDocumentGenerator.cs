using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Feast.JsonAnnotation.Filters;
using Feast.JsonAnnotation.Structs.Code;

namespace Feast.JsonAnnotation.Generators
{
    internal class XmlDocumentGenerator
    {
        private XmlDocument Document { get; } = new();

        public string Root { get; init; } = nameof(Document);

        public string Namespace { get; init; } = nameof(Namespace);

        public string Class { get; init; } = nameof(Class);

        public static XmlDocumentGenerator FromFile<TFilter>(FileRegion<TFilter> region) 
            where TFilter : SyntaxFilter<TFilter>
        {
            
            XmlDocumentGenerator ret = new();
            region.Namespaces.ForEach(x =>
            {
                ret.MapNode();
            });
        }

        private void MapNode()
        {
            Document.CreateElement(Root,"");
        }
    }
}
