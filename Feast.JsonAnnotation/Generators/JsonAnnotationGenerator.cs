using Feast.JsonAnnotation.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;
using System.Linq;
using Feast.JsonAnnotation.Filters;

namespace Feast.JsonAnnotation.Generators
{
    [Generator]
    internal class JsonAnnotationGenerator : ISourceGenerator
    {
        private readonly JsonAttributeFilter receiver = JsonAttributeFilter.Instance;
        public void Initialize(GeneratorInitializationContext context)
        {
            Debugger.Launch();
            context.RegisterForSyntaxNotifications(() => receiver);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (receiver.Generated) return;
            var s = receiver.Results;
            s.ForEach(x =>
            {
                context.AddSource($"{Guid.NewGuid().ToString().Replace('-', '_')}.g.cs", x.Value);
            });
            context.AddSource($"{nameof(JsonAnnotation)}.ext.cs",
                $@"using System;

namespace Feast.JsonAnnotation{{
    public class {nameof(JsonAnnotation)}{{
        public static void {nameof(JsonAnnotation.Generate)}(){{
            
        }}
    }}
}}
");
        }
    }
}
