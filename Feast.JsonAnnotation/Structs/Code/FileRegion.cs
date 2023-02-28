using System;
using System.Collections.Generic;
using System.Text;
using Feast.JsonAnnotation.Extensions;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class FileRegion : CodeRegion
    {
        public List<string> UsingNamespaces { get; set; }

        public Dictionary<string,string> AliasUsingNamespaces { get; set; }

        public List<NamespaceRegion> Namespaces { get; set; }

        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            UsingNamespaces.ForEach(n =>
            {
                sb.AppendLine($"using {n}");
            });
            AliasUsingNamespaces.ForEach(n =>
            {
                sb.AppendLine($"using {n.Key} = {n.Value}");
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
