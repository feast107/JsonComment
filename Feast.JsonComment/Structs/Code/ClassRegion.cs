using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Feast.JsonComment.Extensions;
using Feast.JsonComment.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonComment.Structs.Code
{
    internal class ClassRegion<TFilter> : CodeRegion<TFilter>
        where TFilter : SyntaxFilter<TFilter>
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

        public CommentRegion<TFilter> Comment { get; set; } = new();

        public List<ClassRegion<TFilter>> Classes { get; set; } = new();

        public List<MethodRegion<TFilter>> Methods { get; set; } = new();

        public List<PropertyRegion<TFilter>> Properties { get; set; } = new();

        public List<FieldRegion<TFilter>> Fields { get; set; } = new();

        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            if (Comment.Annotations.Count > 0)
            {
                sb.AppendMultipleLineWithTab(Comment.ContentString(), tab);
            }

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


        public string GetJsonGenerateCode()
        {
            var item = Class.GetFullClassName();

            var thisType = "thisType";
            var instance = "instance";
            var property = "property";

            return $@"
new {nameof(Func<string>)}<{nameof(String)}>(()=>{{
    var {thisType} = typeof({item.Item1}.{item.Item2});
    var {instance} = {typeof(FormatterServices).FullName}.{nameof(FormatterServices.GetUninitializedObject)}({thisType});
     foreach (var {property} in thisType.{nameof(Type.GetProperties)}())
    {{
        if ({property} is not {{ {nameof(PropertyInfo.CanRead)}: true, {nameof(PropertyInfo.CanWrite)}: true }}) break;
{XmlGenerateExtension.GetValueMapper(property,instance).InsertTab()}
    }}
    /*
    var stream = new {typeof(MemoryStream).FullName}();
    new {typeof(DataContractJsonSerializer).FullName}(thisType).{nameof(DataContractJsonSerializer.WriteObject)}(stream, {instance});
    stream.{nameof(Stream.Position)} = 0;    
    var bytes = new byte[stream.{nameof(Stream.Length)}];
    _ = stream.{nameof(Stream.Read)}(bytes, 0, (int)stream.{nameof(Stream.Length)});
    return {typeof(Encoding).FullName}.{nameof(Encoding.UTF8)}.{nameof(Encoding.GetString)}(bytes);
    */
    return System.Text.Json.JsonSerializer.Serialize({instance},new System.Text.Json.JsonSerializerOptions(){{ WriteIndented = true }});
}}).{nameof(Action.Invoke)}()";
        }
    }
}
 