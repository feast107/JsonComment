using Feast.JsonAnnotation.Structs.Code;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonAnnotation.Filters
{
    internal interface ISyntaxFilter<TFilter> where TFilter : ISyntaxFilter<TFilter> 
    {
        /// <summary>
        /// 是否需要这个节点
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool QualifiedClass(ClassDeclarationSyntax syntax, FileRegion<TFilter> context);
        /// <summary>
        /// 是否需要这个节点
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool QualifiedNamespace(BaseNamespaceDeclarationSyntax syntax, FileRegion<TFilter> context);
        /// <summary>
        /// 载入节点后的操作
        /// </summary>
        /// <param name="target"></param>
        void PostClassDeclaration(ClassRegion<TFilter> target);
        /// <summary>
        /// 载入节点后的操作
        /// </summary>
        /// <param name="target"></param>
        void PostNamespaceDeclaration(NamespaceRegion<TFilter> target);
    }
}
