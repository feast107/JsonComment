using System.Xml;
using Feast.JsonComment.Extensions;
using Feast.JsonComment.Filters;
using Feast.JsonComment.Structs.Code;
using Feast.JsonComment.Structs.Doc;

namespace Feast.JsonComment.Generators
{
    internal class XmlDocumentGenerator
    {
        public XmlDocument Document { get; } = new();

        public XmlGenerationConfig Config { get; init; } = new()
        {
            Root = nameof(Document),
            Class = "Class",
            Namespace = "Namespace",
            FileName = "JsonDocument.xml"
        };

        public XmlNodeField Root { get; private set; }

        public string GetGenerateCode(string documentVariable, bool generateDocument) =>
            Root.GetGenerateCode(documentVariable, generateDocument);

        public static XmlDocumentGenerator MapFile<TFilter>(FileRegion<TFilter> region) 
            where TFilter : SyntaxFilter<TFilter>
        {
            XmlDocumentGenerator ret = new();
            ret.Map(region, ret.Config);
            return ret;
        }

        /// <summary>
        /// Map File
        /// </summary>
        /// <typeparam name="TFilter"></typeparam>
        /// <param name="document">XmlDocument</param>
        /// <param name="fileRegion">File</param>
        /// <param name="config">Config</param>
        /// <returns></returns>
        internal void Map<TFilter>(
            FileRegion<TFilter> fileRegion,
            XmlGenerationConfig config)
            where TFilter : SyntaxFilter<TFilter>
        {
            fileRegion.Clip();
            var root = Document.DocumentElement;
            if (root == null)
            {
                root = Document.CreateElement(config.Root);
                Document.AppendChild(root);
            }
            Root = XmlNodeField.NewRootField(Document);
            fileRegion.Namespaces.ForEach(n =>
            {
                Map(Root, n);
            });
        }

        /// <summary>
        /// Map Namespace
        /// </summary>
        /// <typeparam name="TFilter"></typeparam>
        /// <param name="parent"></param>
        /// <param name="namespaceRegion"></param>
        /// <param name="config"></param>
        /// <param name="document"></param>
        internal void Map<TFilter>(
            XmlNodeField parent,
            NamespaceRegion<TFilter> namespaceRegion)
            where TFilter : SyntaxFilter<TFilter>
        {
            var namespaceString = namespaceRegion.Namespace.GetShortNamespace();
            var thisNode = parent.CreateChildField(Config.Namespace, namespaceString);

            namespaceRegion.Classes.ForEach(c =>
            {
                Map(thisNode, c);
            });
            namespaceRegion.Namespaces.ForEach(x =>
            {
                Map(thisNode, x);
            });
        }

        internal void Map<TFilter>(
            XmlNodeField parent,
            ClassRegion<TFilter> classRegion)
            where TFilter : SyntaxFilter<TFilter>
        {
            var classString = classRegion.Class.GetSelfClassName();
            var thisNode = parent.CreateChildField(Config.Class, classString);
            var annotation = thisNode.Annotation(Config.FileName);
            classRegion.Comment.Annotations.Add(annotation);
            thisNode.SetJsonProvider(classRegion.GetJsonGenerateCode());
            classRegion.Classes.ForEach(c => { Map(thisNode, c); });
        }
       
    }
}
