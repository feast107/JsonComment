using Feast.JsonAnnotation.Extensions;
using Feast.JsonAnnotation.Filters;
using Feast.JsonAnnotation.Structs;
using Microsoft.CodeAnalysis;

namespace Feast.JsonAnnotation.Generators
{
    internal class JsonAnnotationReceiver : ISyntaxReceiver 
    {
        public bool Generated { get; set; }

        
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
        }
    }
}
