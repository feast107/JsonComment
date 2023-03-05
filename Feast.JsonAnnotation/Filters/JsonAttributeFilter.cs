using System;
using Feast.JsonAnnotation.Extensions;
using Feast.JsonAnnotation.Structs.Code;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Feast.JsonAnnotation.Generators;
using Feast.JsonAnnotation.Structs.Doc;

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

        public override bool QualifiedClass(ClassDeclarationSyntax syntax, FileRegion<JsonAttributeFilter> context)
        {
            if (!syntax.Modifiers.Has(SyntaxKind.PartialKeyword)) return false;
            return syntax.GetAllAttributeSyntax().Any(x => 
                context.CanReferenceTo(syntax, x, AttributeNamespace, AttributeName));
        }

        public override void BeforeGenerateDoc(FileRegion<JsonAttributeFilter> target)
        {
            var generator = XmlDocumentGenerator.MapFile(target);
            var dir = Path.GetDirectoryName(target.FilePath);
            if (generators.TryGetValue(dir!, out var region))
            {
                var code = generator.GetGenerateCode("dom", false);
                region.Item1.Append(code);
            }
            else
            {
                var code = generator.GetGenerateCode("dom", true);
                region = new(new(code), generator.Config);
                generators[dir] = region;
            }
        }

        private readonly Dictionary<string, Tuple<StringBuilder, XmlGenerationConfig>> generators = new();

        public Dictionary<string, string> Generators =>
            generators.Where(x=>x.Value.Item1.Length != 0)
                .ToDictionary(k => k.Key,
                    v =>
                    {
                        var sb = new StringBuilder();
                        var ret = $@"new {nameof(Action)}(()=>{{
    var dom = new {typeof(XmlDocument).FullName}();
{sb.AppendMultipleLineWith(v.Value.Item1.ToString(),"    ")}
    dom.{nameof(XmlDocument.Save)}(@""{Path.Combine(v.Key, v.Value.Item2.FileName)}"");
}}).{nameof(Action.Invoke)}();";
                        return ret;
                    });

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            CodeRegion<JsonAttributeFilter>.Program.AnalyzeSyntax(syntaxNode);
        }

        public bool Generated => results != null;

        public Dictionary<string, string> Codes => results ??= CodeRegion<JsonAttributeFilter>.Program.ContentStrings;
#nullable enable
        private Dictionary<string, string>? results;
    }
}
