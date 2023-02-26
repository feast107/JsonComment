using System.Diagnostics;
using Feast.JsonAnnotation.Extensions;
using Microsoft.CodeAnalysis;

namespace Feast.JsonAnnotation.Generators
{
    [Generator]
    internal class JsonAnnotationGenerator : ISourceGenerator
    {
        private class SyntaxReceiver : ISyntaxReceiver
        {
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                syntaxNode.ResolveUsing();
                syntaxNode.ResolveDeclare();
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            Debugger.Launch();
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {

        }
    }
}
