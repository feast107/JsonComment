using Feast.JsonAnnotation.Structs;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace Feast.JsonAnnotation.Extensions
{
    internal static class SyntaxResolveExtension
    {
        internal static string FilePath(this SyntaxNode node) => node.SyntaxTree.FilePath;
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
        internal static string GetSelfClassName(this ClassDeclarationSyntax node) => node.Identifier.Text;
        internal static string GetClassName(this ClassDeclarationSyntax node)
        {
            var ret = node.GetSelfClassName();
            var tmp = node.Parent;
            while (tmp is ClassDeclarationSyntax classDeclaration)
            {
                ret = $"{classDeclaration.Identifier.Text}.{ret}";
                tmp = classDeclaration.Parent;
            }
            return ret;
        }
        internal static string GetNamespace(this ClassDeclarationSyntax node)
        {
            var tmp = node.Parent;
            while (tmp is ClassDeclarationSyntax classDeclaration)
            {
                tmp = classDeclaration.Parent;
            }
            var nameSpace = string.Empty;
            if (tmp is BaseNamespaceDeclarationSyntax namespaceDeclaration)
            {
                nameSpace = ((QualifiedNameSyntax)namespaceDeclaration.Name).GetFullUsing();
            }
            return nameSpace;
        }

        internal static bool IsInnerClassOf(this ClassDeclarationSyntax node, ClassDeclarationSyntax another)
        {
            return (another.Equals(node.Parent));
        }
        internal static bool Has(this SyntaxTokenList list, SyntaxKind kind) => list.Any(x => x.IsKind(kind));
        internal static bool Has(this ClassDeclarationSyntax declaration, SyntaxKind kind) => declaration.Modifiers.Any(x => x.IsKind(kind));
        internal static Tuple<string,string> GetFullClassName(this ClassDeclarationSyntax node)
        {
            return new(node.GetNamespace(), node.GetClassName());
        }
        internal static bool ContainsAttribute(this FileScope scope, SyntaxList<AttributeListSyntax> list)
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
