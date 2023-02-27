using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Feast.JsonAnnotation.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonAnnotation.Structs
{
    internal class ClassTree
    {
        public required ClassDeclarationSyntax Node { get; init; }
        public string Prefix => Node.FormatClassName() + " {\n";
        public string Suffix => "}"; 
        private List<ClassTree> Body { get; } = new();
        public string FullString(int tabCount = 0)
        {
            var sb = new StringBuilder();
            sb.Append(Prefix);
            Body.ForEach(s => sb.Append(s.FullString(1)));
            sb.Append(Suffix);
            return sb.ToString().InsertTab(tabCount) + '\n';
        }

        public bool IsOuterClass(ClassTree inner) => inner.Node.IsInnerClassOf(Node);

        public bool TryAppendChild(ClassTree child)
        {
            if (!IsOuterClass(child)) return Body.Any(c => c.TryAppendChild(child));
            AppendChild(child);
            return true;
        }

        private void AppendChild(ClassTree childClass) => Body.Add(childClass);

        public override string ToString() => FullString();
    }
}
