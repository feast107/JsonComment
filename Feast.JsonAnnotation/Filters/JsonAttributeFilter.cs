using Feast.JsonAnnotation.Extensions;
using Feast.JsonAnnotation.Structs.Code;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Feast.JsonAnnotation.Generators;

namespace Feast.JsonAnnotation.Filters
{
    internal class JsonAttributeFilter : SyntaxFilter<JsonAttributeFilter>, ISyntaxReceiver
    {
        public static JsonAttributeFilter Instance { get; } = new();
        private JsonAttributeFilter()
        {
            CodeRegion<JsonAttributeFilter>.Filter = this;
        }

        private string AttributeName { get; } = nameof(JsonAnnotationAttribute).WithoutAttribute();
        private string AttributeNamespace { get; } = typeof(JsonAnnotationAttribute).Namespace;
        private string FullAttributeName { get; } = typeof(JsonAnnotationAttribute).FullName.WithoutAttribute();

        public override bool QualifiedClass(ClassDeclarationSyntax syntax, FileRegion<JsonAttributeFilter> context)
        {
            if (!syntax.Modifiers.Has(SyntaxKind.PartialKeyword)) return false;
            return syntax.GetAllAttributeSyntax().Any(x =>
            {
                var name = x.GetName();
                return name == FullAttributeName
                       || syntax.GetNamespace().StartsWith(AttributeNamespace) && AttributeName == name
                       || context.UsingNamespaces.Any(n => $"{n}.{name}" == FullAttributeName)
                       || context.AliasUsingNamespaces.Any(a =>
                           name.Replace(a.Key, a.Value).WithoutAttribute() == FullAttributeName);
            });
        }

        public override void BeforeGenerateDoc(FileRegion<JsonAttributeFilter> target)
        {
            XmlDocumentGenerator.MapFile(target);
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            CodeRegion<JsonAttributeFilter>.Program.AnalyzeSyntax(syntaxNode);
        }


        public bool Generated => results != null;

        public Dictionary<string, string> Results => results ??= CodeRegion<JsonAttributeFilter>.Program.ContentStrings;
#nullable enable
        private Dictionary<string, string>? results;
    }
}
