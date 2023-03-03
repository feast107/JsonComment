using Feast.JsonAnnotation.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Feast.JsonAnnotation.Filters;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class NamespaceRegion<TFilter> : CodeRegion<TFilter>
        where TFilter : SyntaxFilter<TFilter>
    {
        private FileRegion<TFilter> File { get; }

        public NamespaceRegion(FileRegion<TFilter> file)
        {
            File = file;
        }

        public required BaseNamespaceDeclarationSyntax Namespace
        {
            get => syntaxInternal;
            init
            {
                syntaxInternal = value;
                syntaxInternal.Members.ForEach(x =>
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
                        case BaseNamespaceDeclarationSyntax ns:
                            if (!Filter.QualifiedNamespace(ns, File)) break;
                            Namespaces.Add(new (File)
                            {
                                Namespace = ns,
                            });
                            break;
                    };
                });
                this.PostAction(Filter.PostNamespaceDeclaration);
            }
        }

        private readonly BaseNamespaceDeclarationSyntax syntaxInternal;

        public List<ClassRegion<TFilter>> Classes { get; set; } = new();

        public List<NamespaceRegion<TFilter>> Namespaces { get; set; } = new();

        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            sb.AppendLineWithTab($"namespace {(Namespace.GetShortNamespace())} {{", tab);
            Classes.ForEach(c =>
            {
                sb.AppendMultipleLineWithTab(c.ContentString(DefaultTabCount), tab);
            });
            Namespaces.ForEach(n =>
            {
                sb.AppendMultipleLineWithTab(n.ContentString(DefaultTabCount), tab);
            });
            sb.AppendLineWithTab("}", tab);
            return sb.ToString();
        }

        public bool TryAddNamespace(BaseNamespaceDeclarationSyntax syntax)
        {
            if (Namespace == syntax) return true;
            if (syntax.IsDirectInnerNamespaceOf(Namespace) && Namespaces.All(n => n.Namespace != syntax))
            {
                Namespaces.Add(new(File) { Namespace = syntax });
                return true;
            }
            return Namespaces.Any(n => n.TryAddNamespace(syntax));
        }
    }

   
}
