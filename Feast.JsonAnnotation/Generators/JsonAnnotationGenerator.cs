using Feast.JsonAnnotation.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;

namespace Feast.JsonAnnotation.Generators
{
    [Generator]
    internal class JsonAnnotationGenerator : ISourceGenerator
    {
        private readonly JsonAnnotationReceiver receiver = new();
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => receiver);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (receiver.Generated) return;
            receiver.Generated = true;
            var s = receiver.ClassUsing.TypeUsing.FormatNamespaceStrings();
            s.ForEach(kv =>
            {
                context.AddSource($"{Guid.NewGuid().ToString().Replace('-', '_')}.g.cs", kv.Value);
            });
        }
    }
}
