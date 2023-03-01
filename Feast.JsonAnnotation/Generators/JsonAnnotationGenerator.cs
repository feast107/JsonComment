using Feast.JsonAnnotation.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;
using System.Linq;

namespace Feast.JsonAnnotation.Generators
{
    [Generator]
    internal class JsonAnnotationGenerator : ISourceGenerator
    {
        private readonly JsonAnnotationReceiver receiver = new();
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => receiver);
            Debugger.Launch();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (receiver.Generated) return;
            receiver.Generated = true;
            var s = receiver.TargetUsing.TypeUsing.Select(x => x.Value.GenerateSourceFile());
            s.ForEach(x =>
            {
                context.AddSource($"{Guid.NewGuid().ToString().Replace('-', '_')}.g.cs", x);
            });
            context.AddSource($"{nameof(JsonAnnotation)}.ext.cs",
                $@"using System;

namespace Feast.JsonAnnotation{{
    public class {nameof(JsonAnnotation)}{{
        public static void {nameof(JsonAnnotation.Generate)}(){{
            {typeof(Debugger).FullName}.{nameof(Debugger.Break)}();
        }}
    }}
}}
");
        }
    }
}
