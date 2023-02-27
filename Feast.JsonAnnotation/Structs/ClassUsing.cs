using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using Feast.JsonAnnotation.Extensions;

namespace Feast.JsonAnnotation.Structs
{
    internal readonly struct ClassUsing<TClass>
    {
        internal readonly Type Type = typeof(TClass);
        private readonly Dictionary<string, FileScopeUsing> typeUsing = new();
        public ClassUsing() { }
        internal FileScopeUsing GetUsingSet(string filePath)
        {
            if (!typeUsing.TryGetValue(filePath, out var namespaces))
            {
                namespaces = new(Type);
                typeUsing[filePath] = namespaces;
            }

            var res = typeUsing.FirstOrDefault(x => x.Key != filePath && !x.Value.Used);
            if(res.Key != default){
                typeUsing.Remove(res.Key);
            }
            return namespaces;
        }
        public required Func<ClassDeclarationSyntax, FileScopeUsing,bool> WhetherDeclared { get; init; }
        internal void ResolveUsing(SyntaxNode node)
        {
            if (node is not UsingDirectiveSyntax realNode) return; //Using 声明
            if (!realNode.GetLocation().IsInSource) return;
            if (realNode.Name is not QualifiedNameSyntax subNode) return;
            var str = subNode.GetFullUsing();
            var usingSet = GetUsingSet(realNode.FilePath());
            if (!usingSet.IsQualifiedDeclaration(str)) return;
            usingSet.RegisterAlias(str,
                realNode
                    .Alias?
                    .Name
                    .Identifier
                    .Text);
        }

        internal void ResolveDeclare(SyntaxNode node)
        {
            if (node is not ClassDeclarationSyntax declareNode) return;
            var set = GetUsingSet(node.FilePath());
            var declared = WhetherDeclared(declareNode,set);
            if (declared)
            {
                set.Use();
            }
        }
    }
}
