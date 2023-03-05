using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Feast.JsonComment.Structs.Code;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonComment.Extensions
{
    internal static class StringFormatExtension
    {
        internal static string Repeat(this string src, int time)
        {
            var ret = new StringBuilder();
            while (time > 0)
            {
                ret.Append(src);
                time--;
            }
            return ret.ToString();
        }
        internal static string FormatModifier(this ClassDeclarationSyntax node) => 
            node.Modifiers.Aggregate(new StringBuilder(), (current, modifier) => current.Append(modifier.Text + ' ')).ToString();
        internal static string FormatClassName(this ClassDeclarationSyntax node) => 
            $"{node.FormatModifier()}class {node.GetSelfClassName()}";
        internal static string InsertTab(this string code,int count = 1)
        {
            var tab = "\t".Repeat(count);
            if (code.Length == 0) return tab;
            var last = code[code.Length - 1] == '\n';
            if (last)
            {
                code = code.Remove(code.Length - 1, 1);
            }
            return tab + code.Replace("\n", $"\n{tab}") + (last ? '\n' : "");
        }

        internal static string InsertEachLine(this string code, string prefix)
        {
            if (code.Length == 0) return prefix;
            var last = code[code.Length - 1] == '\n';
            if (last)
            {
                code = code.Remove(code.Length - 1, 1);
            }
            return prefix + code.Replace("\n", $"\n{prefix}") + (last ? '\n' : "");
        }

        internal static string ToCodeString(this string str)
        {
            if (str.Length == 0) return str;
            var head = str[0];
            return head is >= 'A' and <= 'Z' ? $"{(char)(head + 32)}{str.Substring(1)}" : str;
        }
        internal static string ToCodeString(this CodeRegion.AccessModifier modifer)
            => modifer.ToString().ToCodeString();
        internal static string ToCodeString(this CodeRegion.ExtraModifier modifer)
            => modifer.ToString().ToCodeString();

        internal static string WithBlank<T>(this List<T> source, int count = 1)
        {
            var sb = new StringBuilder();
            source.ForEach(x => sb.Append(x + " ".Repeat(count)));
            return sb.ToString();
        }
        internal static string WithBlank<T>(this List<T> source, Func<T,string> formatProvider, int count = 1)
        {
            var sb = new StringBuilder();
            source.ForEach(x => sb.Append(formatProvider(x) + " ".Repeat(count)));
            return sb.ToString();
        }

        internal static string WithoutAttribute(this string className)
        {
            return className.EndsWith(nameof(Attribute)) ? className.Remove(className.Length - 9, 9) : className;
        }

        internal static string MultiLine(this IEnumerable<string> source)
        {
            return MultiLine(source, string.Empty);
        }

        internal static string MultiLine(this IEnumerable<string> source, string prefix)
        {
            var sb = new StringBuilder();
            foreach (var item in source)
            {
                sb.Append(item.InsertEachLine(prefix));
            }
            return sb.ToString();
        }

        internal static StringBuilder AppendLineWithTab(this StringBuilder builder, string content, int tab = 0) =>
            builder.AppendLine($"{"\t".Repeat(tab)}{content}");
        internal static StringBuilder AppendMultipleLineWithTab(this StringBuilder builder, string content, int tab = 0)
        {
            var tabs = "\t".Repeat(tab);
            return builder.AppendMultipleLineWith(content, tabs);
        }

        internal static StringBuilder AppendMultipleLineWith(this StringBuilder builder, string content, string prefix)
        {
            if (content[content.Length - 1] == '\n')
            {
                content = content.Remove(content.Length - 1, 1);
            }
            return builder.Append(prefix)
                .Append(content.Replace("\n", $"\n{prefix}"))
                .Append('\n');
        }
    }
}
