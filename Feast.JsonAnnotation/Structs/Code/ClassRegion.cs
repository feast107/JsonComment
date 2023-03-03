using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Feast.JsonAnnotation.Extensions;
using Feast.JsonAnnotation.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class ClassRegion<TFilter> : CodeRegion<TFilter>
        where TFilter : ISyntaxFilter<TFilter>
    {
        private FileRegion<TFilter> File { get; }

        public ClassRegion(FileRegion<TFilter> file)
        {
            File = file;
        }

        public required ClassDeclarationSyntax Class
        {
            get => syntax;
            init
            {
                syntax = value;
                syntax.Members.ForEach(x =>
                {
                    switch (x)
                    {
                        case ClassDeclarationSyntax clz:
                            if (!Filter.QualifiedClass(clz, File)) break;
                            Classes.Add(new (File)
                            {
                                Class = clz,
                            });
                            break;
                        case PropertyDeclarationSyntax property:
                            /*Properties.Add(new PropertyRegion()
                            {
                                Property = property
                            });*/
                            break;
                    }
                });
                this.PostAction(Filter.PostClassDeclaration);
            }
        }

        private readonly ClassDeclarationSyntax syntax;

        public CodeRegion.AccessModifier Modifier { get; set; } = CodeRegion.AccessModifier.Public;

        public List<CodeRegion.ExtraModifier> ExtraModifiers { get; set; } = new() { CodeRegion.ExtraModifier.Partial };

        public List<AnnotationRegion<TFilter>> Annotations { get; set; } = new()
        {
            new AnnotationRegion<TFilter>()
        };

        public List<ClassRegion<TFilter>> Classes { get; set; } = new();

        public List<MethodRegion<TFilter>> Methods { get; set; } = new();

        public List<PropertyRegion<TFilter>> Properties { get; set; } = new();

        public List<FieldRegion<TFilter>> Fields { get; set; } = new();

        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            Annotations.ForEach(a =>
            {
                sb.AppendMultipleLineWithTab(a.ContentString(),tab);
            });

            sb.AppendLineWithTab(
                $"{ExtraModifiers.WithBlank(StringFormatExtension.ToCodeString)}" +
                $"class " +
                $"{Class.GetSelfClassName()} {{", tab);
            Classes.ForEach(c =>
            {
                sb.AppendMultipleLineWithTab(c.ContentString(DefaultTabCount), tab);
            });

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
