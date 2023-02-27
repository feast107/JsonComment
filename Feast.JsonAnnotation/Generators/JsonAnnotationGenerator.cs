using System.Diagnostics;
using Feast.JsonAnnotation.Extensions;
using Feast.JsonAnnotation.Structs;
using Microsoft.CodeAnalysis;

namespace Feast.JsonAnnotation.Generators
{
    [Generator]
    internal class JsonAnnotationGenerator : ISourceGenerator
    {

        public void Initialize(GeneratorInitializationContext context)
        {
            Debugger.Launch();
            context.RegisterForSyntaxNotifications(() => new JsonAnnotationReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
        }
    }
}
