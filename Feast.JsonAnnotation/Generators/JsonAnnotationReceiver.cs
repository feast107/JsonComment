using Feast.JsonAnnotation.Extensions;
using Feast.JsonAnnotation.Structs;
using Microsoft.CodeAnalysis;

namespace Feast.JsonAnnotation.Generators
{
    internal class JsonAnnotationReceiver : ISyntaxReceiver 
    {
        private readonly ClassUsing<JsonAnnotationAttribute> classUsing = new()
        {
            WhetherDeclared = (node, set) => set.ContainsAttribute(node.AttributeLists)
        };

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            classUsing.ResolveUsing(syntaxNode);
            classUsing.ResolveDeclare(syntaxNode);
        }
    }
}
