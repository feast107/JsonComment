using Feast.JsonAnnotation.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;
using Feast.JsonAnnotation.Filters;

namespace Feast.JsonAnnotation.Generators
{
    [Generator]
    internal class JsonAnnotationGenerator : ISourceGenerator
    {
        private readonly JsonAttributeFilter receiver = JsonAttributeFilter.Instance;
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => receiver);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (receiver.Generated) return;
            var s = receiver.Codes;
            
            s.ForEach(x =>
            {
                context.AddSource($"{Guid.NewGuid().ToString().Replace('-', '_')}.g.cs", x.Value);
            });

            var generateCode = $@"using System;

namespace Feast.JsonAnnotation{{
    public class {nameof(JsonAnnotation)}{{
        public static void {nameof(JsonAnnotation.Generate)}(){{
{receiver.Generators.Values.MultiLine("            ")}
        }}
    }}
}}";
            context.AddSource($"{nameof(JsonAnnotation)}.ext.cs", generateCode);
        }
    }
}
