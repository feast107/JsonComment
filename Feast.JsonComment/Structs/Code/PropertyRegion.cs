using System;
using Feast.JsonComment.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonComment.Structs.Code
{
    internal class PropertyRegion<TFilter> : CodeRegion<TFilter>
        where TFilter : SyntaxFilter<TFilter>
    {
        public required PropertyDeclarationSyntax Property { get; init; }

        public override string ContentString(int tab = 0)
        {
            throw new NotImplementedException();
        }
    }
}
