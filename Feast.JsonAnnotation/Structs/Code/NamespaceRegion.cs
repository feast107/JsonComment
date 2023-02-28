using System;
using System.Collections.Generic;
using System.Text;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class NamespaceRegion : CodeRegion
    {
        public required string Namespace { get; init; }

        public List<ClassRegion> Classes { get; set; } = new();

        public List<NamespaceRegion> Namespaces { get; set; } = new();

        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"namespace {Namespace} {{");
            Classes.ForEach(c =>
            {
                sb.AppendLine(c.ContentString(DefaultTabCount));
            });
            Namespaces.ForEach(n =>
            {
                sb.AppendLine(n.ContentString(DefaultTabCount));
            });
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
