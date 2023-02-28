using System;
using System.Collections.Generic;
using System.Text;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class AnnotationRegion : CodeRegion
    {
        public List<string> Annotations = new();
        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            sb.AppendLine("/// <summary>");
            Annotations.ForEach(x => sb.AppendLine($"/// {x}"));
            sb.Append("/// </summary>");
            return sb.ToString();
        }
    }
}
