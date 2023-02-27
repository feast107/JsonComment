using Feast.JsonAnnotation.Extensions;
using Feast.JsonAnnotation.Structs;
using Microsoft.CodeAnalysis;

namespace Feast.JsonAnnotation.Generators
{
    internal class JsonAnnotationReceiver : ISyntaxReceiver 
    {
        internal readonly ClassScope<JsonAnnotationAttribute> ClassUsing = new()
        {
            WhetherDeclared = (node, set) => set.ContainsAttribute(node.AttributeLists)
        };

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            ClassUsing.ResolveUsing(syntaxNode);
            ClassUsing.ResolveNamespaceDeclare(syntaxNode);
            ClassUsing.ResolveClassDeclare(syntaxNode);
        }
    }
}
