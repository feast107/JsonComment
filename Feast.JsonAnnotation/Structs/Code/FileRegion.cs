using Feast.JsonAnnotation.Extensions;
using System.Collections.Generic;
using System.Text;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class FileRegion : CodeRegion
    {
        public List<string> UsingNamespaces { get; set; } = new()
        {
            nameof(System)
        };

        public Dictionary<string,string> AliasUsingNamespaces { get; set; } = new();

        public List<NamespaceRegion> Namespaces { get; set; } = new();

        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            UsingNamespaces.ForEach(n =>
            {
                sb.AppendLine($"using {n};");
            });
            AliasUsingNamespaces.ForEach(n =>
            {
                sb.AppendLine($"using {n.Key} = {n.Value};");
            });
            
            sb.AppendLine();

            Namespaces.ForEach(n =>
            {
                sb.AppendLine(n.ContentString());
            });

            return sb.ToString();
        }
    }
}
