using Feast.JsonAnnotation.Attributes;
using Feast.JsonAnnotation.Structs;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Feast.JsonAnnotation.Extensions
{
    internal static class SyntaxResolveExtension
    {
        internal static readonly Dictionary<string, FileScopeUsing> UsingCache = new();
        private static FileScopeUsing GetUsingSet(string filePath)
        {
            if (UsingCache.TryGetValue(filePath, out var namespaces)) return namespaces;
            namespaces = new(typeof(JsonAnnotationAttribute));
            UsingCache[filePath] = namespaces;
            return namespaces;
        }
        internal static string GetFullUsing(this QualifiedNameSyntax node)
        {
            var ret = node.Right.Identifier.Text;
            var left = node.Left;
            while (left is QualifiedNameSyntax qualified)
            {
                ret = $"{qualified.Right.Identifier.Text}.{ret}";
                left = qualified.Left;
            }
            if (left is IdentifierNameSyntax identifier)
            {
                ret = $"{identifier.Identifier.Text}.{ret}";
            }
            return ret;
        }
        internal static void ResolveUsing(this SyntaxNode node)
        {
            if (node is not UsingDirectiveSyntax realNode) return; //Using 声明
            if (!realNode.GetLocation().IsInSource) return;
            if (realNode.Name is not QualifiedNameSyntax subNode) return;
            var str = subNode.GetFullUsing();
            var usingSet = GetUsingSet(realNode.SyntaxTree.FilePath);
            if (!usingSet.IsQualifiedDeclaration(str)) return;
            usingSet.RegisterAlias(str,
                realNode
                    .Alias?
                    .Name
                    .Identifier
                    .Text);
        }
        internal static void ResolveDeclare(this SyntaxNode node)
        {
            if (node is not ClassDeclarationSyntax declareNode) return;
            SyntaxList<AttributeListSyntax> attrs = declareNode.AttributeLists;
            var check = attrs.ContainsAttribute(GetUsingSet(node.SyntaxTree.FilePath));
        }
        internal static bool ContainsAttribute(this SyntaxList<AttributeListSyntax> list, FileScopeUsing scope)
        {
            return list
                .Any(node =>
                    node
                        .Attributes
                        .Any(attr => attr.Name switch
                        {
                            QualifiedNameSyntax syntax => scope.HasAttribute(syntax.Right.Identifier.Text),
                            IdentifierNameSyntax syntax => scope.HasAttribute(syntax.Identifier.Text),
                            _ => false,
                        })
                );
        }
    }
}
