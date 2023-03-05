using System;
using Feast.JsonComment.Extensions;
using Feast.JsonComment.Filters;
using Microsoft.CodeAnalysis;

namespace Feast.JsonComment.Generators
{
    [Generator]
    internal class JsonCommentGenerator : ISourceGenerator
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
                context.AddSource((string)$"{Guid.NewGuid().ToString().Replace('-', '_')}.g.cs", (string)x.Value);
            });

            var generateCode = $@"using System;

namespace {nameof(Feast)}.{nameof(JsonComment)}{{
    public class {nameof(JsonComment)}{{
        public static void {nameof(JsonComment.Generate)}(){{
{receiver.Generators.Values.MultiLine("            ")}
        }}
    }}
}}";
            context.AddSource($"{nameof(JsonComment)}.ext.cs", generateCode);
        }
    }
}
