using Feast.JsonAnnotation.Structs;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feast.JsonAnnotation.Extensions
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
        internal static string FormatClassesString(this IEnumerable<ClassTree> trees,int tabCount = 0)
        {
            var ret = new StringBuilder();
            foreach (var tree in trees)
            {
                ret.Append(tree.FullString(tabCount));
            }
            return ret.ToString();
        }
        internal static string FormatNamespaceString(this string nameSpace, HashSet<ClassDeclarationSyntax> classes)
        {
            List<ClassTree> roots = new();
            classes.ForEach(z =>
            {
                var clazz = new ClassTree() { Node = z };
                if (!roots.Any(root => root.TryAppendChild(clazz)))
                {
                    roots.Add(clazz);
                }
            });
            return $"namespace { nameSpace }{{\n{roots.FormatClassesString().InsertTab(1)}}}";
        }
        internal static string FormatNamespaceStrings(this Dictionary<string, FileScope> namespaces)
        {
            var ret = new StringBuilder();
            foreach (var c in namespaces.SelectMany(pair => pair.Value.UsingClass))
            {
                ret.Append(c.Key.FormatNamespaceString(c.Value) + '\n');
            }
            return ret.ToString();
        }
        internal static string InsertTab(this string code,int count = 1)
        {
            var tab = "\t".Repeat(count);
            return tab + code.Replace("\n", $"\n{tab}");
        }
    }
}
