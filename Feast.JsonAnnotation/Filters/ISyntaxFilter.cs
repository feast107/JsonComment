using Feast.JsonAnnotation.Structs.Code;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonAnnotation.Filters
{
    internal interface ISyntaxFilter<TFilter> where TFilter : ISyntaxFilter<TFilter> 
    {
        bool QualifiedClass(ClassDeclarationSyntax syntax, FileRegion<TFilter> context);

        bool QualifiedNamespace(BaseNamespaceDeclarationSyntax syntax, FileRegion<TFilter> context);
    }
}
