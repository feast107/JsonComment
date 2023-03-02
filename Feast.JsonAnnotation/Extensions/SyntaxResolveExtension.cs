using Feast.JsonAnnotation.Structs;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using Feast.JsonAnnotation.Structs.Code;
using Microsoft.CodeAnalysis.CSharp;


namespace Feast.JsonAnnotation.Extensions
{
    internal static class SyntaxResolveExtension
    {
        internal static string FilePath(this SyntaxNode syntax) => syntax.SyntaxTree.FilePath;

        internal static string GetNamespace(this BaseNamespaceDeclarationSyntax syntax)
        {
            return syntax.Name switch
            {
                IdentifierNameSyntax identifier => identifier.Identifier.Text,
                QualifiedNameSyntax qualifiedName => qualifiedName.GetFullName(),
                _ => string.Empty
            };
        }
        internal static string GetFullName(this QualifiedNameSyntax syntax)
        {
            var ret = syntax.Right.Identifier.Text;
            var left = syntax.Left;
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
        internal static string GetSelfClassName(this ClassDeclarationSyntax syntax) => syntax.Identifier.Text;
        internal static string GetClassName(this ClassDeclarationSyntax syntax)
        {
            var ret = syntax.GetSelfClassName();
            var tmp = syntax.Parent;
            while (tmp is ClassDeclarationSyntax classDeclaration)
            {
                ret = $"{classDeclaration.Identifier.Text}.{ret}";
                tmp = classDeclaration.Parent;
            }
            return ret;
        }
        internal static string GetNamespace(this ClassDeclarationSyntax syntax)
        {
            var tmp = syntax.Parent;
            while (tmp is ClassDeclarationSyntax classDeclaration)
            {
                tmp = classDeclaration.Parent;
            }
            var nameSpace = string.Empty;
            if (tmp is BaseNamespaceDeclarationSyntax namespaceDeclaration)
            {
                nameSpace = ((QualifiedNameSyntax)namespaceDeclaration.Name).GetFullName();
            }
            return nameSpace;
        }

        internal static bool MayUse(this string namespacePrefix, Type type)
        {
            return type.FullName!.StartsWith(namespacePrefix);
        }
        internal static bool MayUse<TAttribute>(this string namespacePrefix)
        where TAttribute : Attribute
        {
            return namespacePrefix.StartsWith(typeof(TAttribute).FullName!);
        }

        internal static List<AttributeSyntax> GetAllAttributeSyntax(this ClassDeclarationSyntax syntax)
        {
            var result = new List<AttributeSyntax>();
            foreach (var x in syntax.AttributeLists) 
                result.AddRange(x.Attributes);
            return result.Distinct().ToList();
        }

        internal static bool IsDirectInnerNamespaceOf(this BaseNamespaceDeclarationSyntax syntax,
            BaseNamespaceDeclarationSyntax another) => syntax.Parent == another;

        internal static bool IsInnerNamespaceOf(this BaseNamespaceDeclarationSyntax syntax,
            BaseNamespaceDeclarationSyntax another)
        {
            var parent = syntax.Parent;
            while (parent is BaseNamespaceDeclarationSyntax par)
            {
                if (another.Equals(par)) return true;
                parent = par.Parent;
            }
            return false;
        }

        internal static bool IsDirectInnerClassOf(this ClassDeclarationSyntax syntax, ClassDeclarationSyntax another) => syntax.Parent == another;
        internal static bool IsInnerClassOf(this ClassDeclarationSyntax syntax, ClassDeclarationSyntax another)
        {
            var parent = syntax.Parent;
            while (parent is ClassDeclarationSyntax par)
            {
                if (another.Equals(par)) return true;
                parent = par.Parent;
            }
            return false;
        }
        internal static bool Has(this SyntaxTokenList list, SyntaxKind kind) => list.Any(x => x.IsKind(kind));
        internal static bool Has(this ClassDeclarationSyntax declaration, SyntaxKind kind) => declaration.Modifiers.Any(x => x.IsKind(kind));
        internal static Tuple<string,string> GetFullClassName(this ClassDeclarationSyntax syntax)
        {
            return new(syntax.GetNamespace(), syntax.GetClassName());
        }
        internal static bool ContainsAttribute<T>(this FileScope<T> scope, SyntaxList<AttributeListSyntax> list)
        {
            return list
                .Any(syntax =>
                    syntax
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
