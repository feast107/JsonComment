using Feast.JsonAnnotation.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feast.JsonAnnotation.Structs
{

    internal class ClassTree
    {
        public required ClassDeclarationSyntax Node { get; init; }
        public string Annotations { get; set; } = 
            $"/// <summary>\r\n" +
            $"/// <include file='Folder/Document.xml' path='Document/Model[@name=\"Model\"]'/>\r\n" +
            $"/// <include file='../../Folder/Document.xml' path='Document/Model[@name=\"Model\"]'/>\r\n" +
            $"/// </summary>" +
            $"\n";

        public string Prefix => Node.FormatClassName() + " {\n";
        public string Suffix => "}"; 
        private List<ClassTree> Body { get; } = new();

#nullable enable
        private string? contentInternal;
        public string FullString(int tabCount = 0)
        {
            if (contentInternal != null)return contentInternal;
            var sb = new StringBuilder();
            sb.Append(Annotations);
            sb.Append(Prefix);
            Body.ForEach(s => sb.Append(s.FullString(1)));
            sb.Append("public string Generate(){\n" +
                      "     return System.Text.Json.JsonSerializer.Serialize(this);\n" +
                      "}".InsertTab()+'\n');

            sb.Append("public string FilePath() => new System.Diagnostics.StackTrace(true).GetFrame(0)!.GetFileName();\n");
            sb.Append(Suffix);
            return contentInternal = sb.ToString().InsertTab(tabCount) + '\n';
        }

        public bool Generated => contentInternal != null;

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
