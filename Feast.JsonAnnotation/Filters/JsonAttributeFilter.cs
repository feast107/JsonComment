using System;
using System.Collections.Generic;
using System.Text;
using Feast.JsonAnnotation.Structs.Code;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonAnnotation.Filters
{
    internal class JsonAttributeFilter : ISyntaxFilter<JsonAttributeFilter> , ISyntaxReceiver
    {
        public static JsonAttributeFilter Instance { get; }= new();
        private JsonAttributeFilter()
        {
            CodeRegion<JsonAttributeFilter>.Filter = this;
        }


        public bool QualifiedClass(ClassDeclarationSyntax syntax, FileRegion<JsonAttributeFilter> context)
        {
            return true;
        }

        public bool QualifiedNamespace(BaseNamespaceDeclarationSyntax syntax, FileRegion<JsonAttributeFilter> context)
        {
            return true;
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            CodeRegion<JsonAttributeFilter>.Program.AnalyzeSyntax(syntaxNode);
        }

        public bool Generated => results != null;

        public Dictionary<string, string> Results => results ??= CodeRegion<JsonAttributeFilter>.Program.ContentStrings;
#nullable enable
        private Dictionary<string,string>? results ;
    }
}
