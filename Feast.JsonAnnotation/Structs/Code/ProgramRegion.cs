using Feast.JsonAnnotation.Filters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class ProgramRegion<TFilter> : CodeRegion<TFilter>
    where TFilter : SyntaxFilter<TFilter>
    {
        private Dictionary<string, FileRegion<TFilter>> Contexts { get; } = new();
        public FileRegion<TFilter> GetContext(SyntaxNode node) => GetContext(node.SyntaxTree.FilePath);
        private FileRegion<TFilter> GetContext(string filePath)
        {
            if (Program.Contexts.TryGetValue(filePath, out var context)) return context;
            context = new() { FilePath = filePath };
            Program.Contexts.Add(filePath, context);
            return context;
        }

        public void AnalyzeSyntax(SyntaxNode node)
        {
            switch (node)
            {
                case UsingDirectiveSyntax usingS:
                    GetContext(usingS).UsingNamespace(usingS);
                    break;
                case BaseNamespaceDeclarationSyntax nameSpace:
                    GetContext(nameSpace).DeclareNamespace(nameSpace);
                    break;
            }
        }

        public override string ContentString(int tab = 0)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> ContentStrings =>
            Contexts.Where(x=>
                {
                    if(x.Value.Namespaces.Count == 0) { return false; }

                    x.Value.Namespaces.RemoveAll(n => n.Classes.Count == 0 &&
                                                      n.Namespaces.Count == 0);
                    return x.Value.Namespaces.Count != 0;
                })
                .ToDictionary(k => 
                    k.Key, v => v.Value.ContentString());
    }
}
