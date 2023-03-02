using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Feast.JsonAnnotation.Filters;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class PropertyRegion<TFilter> : CodeRegion<TFilter>
        where TFilter : ISyntaxFilter<TFilter>
    {
        public required PropertyDeclarationSyntax Property { get; init; }

        public override string ContentString(int tab = 0)
        {
            throw new NotImplementedException();
        }
    }
}
