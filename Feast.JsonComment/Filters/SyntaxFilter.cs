using Feast.JsonComment.Structs.Code;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonComment.Filters
{
    internal abstract class SyntaxFilter<TFilter> where TFilter : SyntaxFilter<TFilter>
    {
        /// <summary>
        /// 是否需要这个节点
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool QualifiedClass(ClassDeclarationSyntax syntax, FileRegion<TFilter> context) => true;

        /// <summary>
        /// 是否需要这个节点
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool QualifiedNamespace(BaseNamespaceDeclarationSyntax syntax, FileRegion<TFilter> context) => true;

        /// <summary>
        /// 载入节点后的操作
        /// </summary>
        /// <param name="target"></param>
        public virtual void PostClassDeclaration(ClassRegion<TFilter> target) { }

        /// <summary>
        /// 载入节点后的操作
        /// </summary>
        /// <param name="target"></param>
        public virtual void PostNamespaceDeclaration(NamespaceRegion<TFilter> target) { }

        /// <summary>
        /// 文档即将生成前操作
        /// </summary>
        /// <param name="target"></param>
        public virtual void BeforeGenerateDoc(FileRegion<TFilter> target) { }
        
    }
}
