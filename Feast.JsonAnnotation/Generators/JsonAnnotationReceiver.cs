using Feast.JsonAnnotation.Extensions;
using Feast.JsonAnnotation.Structs;
using Microsoft.CodeAnalysis;

namespace Feast.JsonAnnotation.Generators
{
    internal class JsonAnnotationReceiver : ISyntaxReceiver 
    {
        public bool Generated { get; set; }

        internal readonly TargetScope<JsonAnnotationAttribute> TargetUsing = new()
        {
            WhetherDeclared = (node, set) => set.ContainsAttribute(node.AttributeLists)
        };

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            TargetUsing.ResolveUsing(syntaxNode);
            TargetUsing.ResolveNamespaceDeclare(syntaxNode);
            TargetUsing.ResolveClassDeclare(syntaxNode);
        }
    }
}
