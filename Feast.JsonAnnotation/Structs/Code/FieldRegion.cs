﻿using Feast.JsonAnnotation.Extensions;
using Feast.JsonAnnotation.Filters;
using System.Text;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class FieldRegion<TFilter> : CodeRegion<TFilter> 
        where TFilter : SyntaxFilter<TFilter>
    {
        public CodeRegion.AccessModifier Modifier { get; set; }

        public required string Name { get; set; }

        public required string Type { get; set; }
#nullable enable
        public required string? Value { get; set; }

        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{Modifier.ToCodeString()} {Type} {Name} {(Value != null ? "= " + Value : "")};");
            return sb.ToString();
        }
    }
}
