using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Feast.JsonAnnotation.Filters;
using Feast.JsonAnnotation.Structs;
using Feast.JsonAnnotation.Structs.Code;
using Feast.JsonAnnotation.Structs.Doc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonAnnotation.Extensions
{
    internal static class XmlGenerateExtension
    {
        /// <summary>
        /// Map File
        /// </summary>
        /// <typeparam name="TFilter"></typeparam>
        /// <param name="document">XmlDocument</param>
        /// <param name="fileRegion">File</param>
        /// <param name="config">Config</param>
        /// <returns></returns>
        internal static void Map<TFilter>(
            this XmlDocument document,
            FileRegion<TFilter> fileRegion,
            XmlNodeConfig config)
            where TFilter : SyntaxFilter<TFilter>
        {
            var root = document.DocumentElement;
            if (root == null)
            {
                root = document.CreateElement(config.Root);
                document.AppendChild(root);
            }
            var node = new XmlNodeField() { Element = root, };
            fileRegion.Namespaces.ForEach(n =>
            {
                node.Map(n, config, document);
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
        internal static void Map<TFilter>(
            this XmlNodeField parent, 
            NamespaceRegion<TFilter> namespaceRegion,
            XmlNodeConfig config,
            XmlDocument document)
            where TFilter : SyntaxFilter<TFilter>
        {
            var namespaceString = namespaceRegion.Namespace.GetFullNamespace();
            var thisNode = new XmlNodeField()
            {
                Element = parent.CreateChildElement(document, config.Namespace, namespaceString),
                Parent = parent
            };




            namespaceRegion.Classes.ForEach(c =>
            {
                thisNode.Map(c, config, document);
            });
            namespaceRegion.Namespaces.ForEach(x =>
            {
                thisNode.Map(x, config, document);
            });
        }

        internal static void Map<TFilter>(
            this XmlNodeField parent,
            ClassRegion<TFilter> classRegion,
            XmlNodeConfig config,
            XmlDocument document)
            where TFilter : SyntaxFilter<TFilter>
        {
            var classString = classRegion.Class.GetSelfClassName();
            var thisNode = new XmlNodeField()
            {
                Element = parent.CreateChildElement(document, config.Class, classString),
                Parent = parent
            };
            var str = thisNode.ToString();


            classRegion.Classes.ForEach(c => { thisNode.Map(c, config, document); });
        }
    }
}
