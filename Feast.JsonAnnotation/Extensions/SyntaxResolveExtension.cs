using Feast.JsonAnnotation;
using Feast.JsonAnnotation.Structs;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

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
        internal static bool ContainsAttribute(this FileScopeUsing scope, SyntaxList<AttributeListSyntax> list)
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
