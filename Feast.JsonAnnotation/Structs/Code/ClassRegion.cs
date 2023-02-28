using System;
using System.Collections.Generic;
using System.Text;
using Feast.JsonAnnotation.Extensions;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class ClassRegion : CodeRegion
    {
        public required string ClassName { get; init; }

        public AccessModifier Modifier { get; set; } = AccessModifier.Public;

        public List<ExtraModifier> ExtraModifiers { get; set; } = new() { ExtraModifier.Partial };

        public List<AnnotationRegion> Annotations { get; set; } = new();

        public List<MethodRegion> Methods { get; set; } = new();

        public List<PropertyRegion> Properties { get; set; } = new();

        public List<FieldRegion> Fields { get; set; } = new();

        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            Annotations.ForEach(a =>
            {
                sb.AppendLineWithTab(a.ContentString(0),tab);
            });

            sb.AppendLineWithTab($"{Modifier.ToCodeString()} " +
                          $"{ExtraModifiers.WithBlank(StringFormatExtension.ToCodeString)} " +
                          $"{ClassName} {{", tab);

            Fields.ForEach(f =>
            {
                sb.AppendLineWithTab(f.ContentString(DefaultTabCount), tab);
            });
            Properties.ForEach(p =>
            {
                sb.AppendLineWithTab(p.ContentString(DefaultTabCount), tab);
            });
            Methods.ForEach(m =>
            {
                sb.AppendLineWithTab(m.ContentString(DefaultTabCount), tab);
            });
            sb.AppendLineWithTab("}", tab);

            return sb.ToString();
        }
    }
}
