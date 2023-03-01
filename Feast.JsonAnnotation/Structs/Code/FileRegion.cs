using System;
using Feast.JsonAnnotation.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class FileRegion : CodeRegion
    {
        public required string FilePath { get; set; }

        public List<string> UsingNamespaces { get; set; } = new()
        {
            nameof(System)
        };

        public Dictionary<string,string> AliasUsingNamespaces { get; set; } = new();

        public List<NamespaceRegion> Namespaces { get; set; } = new();

        /// <summary>
        /// Using命名空间
        /// </summary>
        /// <param name="syntax"></param>
        public void UsingNamespace(UsingDirectiveSyntax syntax)
        {
            if (syntax.Alias is not null && !AliasUsingNamespaces.ContainsKey(syntax.Alias.Name.Identifier.Text))
            {
                AliasUsingNamespaces.Add(
                    syntax.Alias.Name.Identifier.Text,
                    (syntax.Name as QualifiedNameSyntax).GetFullName());
            }
            else
            {
                UsingNamespaces.Add((syntax.Name as QualifiedNameSyntax).GetFullName());
            }
        }

        /// <summary>
        /// 声明命名空间
        /// </summary>
        /// <param name="syntax"></param>
        public void DeclareNamespace(BaseNamespaceDeclarationSyntax syntax)
        {
            var nameSpace = (syntax.Name as QualifiedNameSyntax).GetFullName();
            if (Namespaces.Any(x => x.Namespace == nameSpace))
            {
                return;
            }
            Namespaces.Add(new() { Namespace = nameSpace });
        }

        /// <summary>
        /// 在当前文件中能否引用到目标类型
        /// </summary>
        /// <param name="syntax">提供的命名节点</param>
        /// <param name="targetNamespace">目标的命名空间</param>
        /// <param name="targetClassname">目标的类型</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CanReferenceTo(QualifiedNameSyntax syntax,string targetNamespace,string targetClassname)
        {
            var name = syntax.GetFullName();
            if (UsingNamespaces.Any(n => n == name)) return true;
            throw new NotImplementedException();
        }
        public override string ContentString(int tab = 0)
        {
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
    }
}
