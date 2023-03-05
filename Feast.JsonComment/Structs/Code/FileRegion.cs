using System.Collections.Generic;
using System.Linq;
using System.Text;
using Feast.JsonComment.Extensions;
using Feast.JsonComment.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonComment.Structs.Code
{
    internal class FileRegion<TFilter> : CodeRegion<TFilter>
        where TFilter : SyntaxFilter<TFilter>
    {
        public required string FilePath { get; set; }

        public List<string> UsingNamespaces { get; set; } = new()
        {
            nameof(System)
        };

        public Dictionary<string,string> AliasUsingNamespaces { get; set; } = new();

        public List<NamespaceRegion<TFilter>> Namespaces { get; set; } = new();

        /// <summary>
        /// Using命名空间
        /// </summary>
        /// <param name="syntax"></param>
        public void UsingNamespace(UsingDirectiveSyntax syntax)
        {
            if (syntax.Name is not QualifiedNameSyntax nameSyntax) return;
            var name = nameSyntax.GetFullName();
            if (syntax.Alias is not null && !AliasUsingNamespaces.ContainsKey(syntax.Alias.Name.Identifier.Text))
            {
                AliasUsingNamespaces.Add(
                    syntax.Alias.Name.Identifier.Text,
                    name.WithoutAttribute());
            }
            else if(UsingNamespaces.All(x => x != name))
            {
                UsingNamespaces.Add(name);
            }
        }

        /// <summary>
        /// 声明命名空间
        /// </summary>
        /// <param name="syntax"></param>
        public void DeclareNamespace(BaseNamespaceDeclarationSyntax syntax)
        {
            if (!Filter.QualifiedNamespace(syntax, this)) return ;
            if (Namespaces.Any(x => x.Namespace == syntax || x.TryAddNamespace(syntax)))
            {
                return;
            }
            Namespaces.Add(new (this)
            {
                Namespace = syntax,
            });
        }

        public override string ContentString(int tab = 0)
        {
            Filter.BeforeGenerateDoc(this);
            var sb = new StringBuilder();
            UsingNamespaces.ForEach(n =>
            {
                sb.AppendLine($"using {n};");
            });
            AliasUsingNamespaces.ForEach(n =>
            {
                sb.AppendLine($"using {n.Key} = {n.Value};");
            });
            
            sb.AppendLine();

            Namespaces.ForEach(n =>
            {
                sb.AppendLine(n.ContentString());
            });

            return sb.ToString();
        }

        public override bool Clip()
        {
            var index = 0;
            while (index < Namespaces.Count)
            {
                if (Namespaces[index].Clip())
                {
                    Namespaces.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
            return true;
        }
    }

    
}
