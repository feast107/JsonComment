using System;
using System.Collections.Generic;
using System.Text;
using Feast.JsonAnnotation.Extensions;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class FieldRegion : CodeRegion
    {
        public AccessModifier AccessModifier { get; set; }

        public required string Name { get; set; }

        public required string Type { get; set; }
#nullable enable
        public required string? Value { get; set; }

        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{AccessModifier.ToCodeString()} {Type} {Name} {(Value != null ? "= " + Value : "")};");
            return sb.ToString();
        }
    }
}
