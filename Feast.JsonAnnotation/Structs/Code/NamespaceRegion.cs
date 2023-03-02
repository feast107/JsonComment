using Feast.JsonAnnotation.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Feast.JsonAnnotation.Filters;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class NamespaceRegion<TFilter> : CodeRegion<TFilter>
        where TFilter : ISyntaxFilter<TFilter>
    {
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
                            if (!Filter.QualifiedClass(clz, Context(clz))) break;
                            Classes.Add(new ()
                            {
                                Class = clz,
                            });
                            break;
                        case BaseNamespaceDeclarationSyntax ns:
                            if (!Filter.QualifiedNamespace(ns, Context(ns))) break;
                            Namespaces.Add(new ()
                            {
                                Namespace = ns
                            });
                            break;
                    };
                });
            }
        }

        private readonly BaseNamespaceDeclarationSyntax syntaxInternal;

        public List<ClassRegion<TFilter>> Classes { get; set; } = new();

        public List<NamespaceRegion<TFilter>> Namespaces { get; set; } = new();

        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            sb.AppendLineWithTab($"namespace {(Namespace.GetNamespace())} {{", tab);
            Classes.ForEach(c =>
            {
                sb.AppendLineWithTab(c.ContentString(DefaultTabCount), tab);
            });
            Namespaces.ForEach(n =>
            {
                sb.AppendLineWithTab(n.ContentString(DefaultTabCount), tab);
            });
            sb.AppendLineWithTab("}", tab);
            return sb.ToString();
        }

        public bool TryAddNamespace(BaseNamespaceDeclarationSyntax syntax)
        {
            if (!Filter.QualifiedNamespace(syntax, Context(syntax))) return false;
            if (Namespace == syntax) return true;
            if (syntax.IsDirectInnerNamespaceOf(Namespace) && Namespaces.All(n => n.Namespace != syntax))
            {
                Namespaces.Add(new() { Namespace = syntax });
                return true;
            }
            return Namespaces.Any(n => n.TryAddNamespace(syntax));
        }
    }

   
}
