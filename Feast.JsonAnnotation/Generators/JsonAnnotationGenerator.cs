using System.Diagnostics;
using Feast.JsonAnnotation.Extensions;
using Feast.JsonAnnotation.Structs;
using Microsoft.CodeAnalysis;

namespace Feast.JsonAnnotation.Generators
{
    [Generator]
    internal class JsonAnnotationGenerator : ISourceGenerator
    {
        private readonly JsonAnnotationReceiver receiver = new();
        public void Initialize(GeneratorInitializationContext context)
        {
            Debugger.Launch();
            context.RegisterForSyntaxNotifications(() => receiver);
        }

        public void Execute(GeneratorExecutionContext context)
        {

        }
    }
}
