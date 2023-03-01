using Feast.JsonAnnotation.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feast.JsonAnnotation.Structs.Code;

namespace Feast.JsonAnnotation.Structs
{
    internal readonly struct TargetScope<TClass>
    {
        #region Fields
        public TargetScope() { }
        /// <summary>
        /// Key:源文件,Value域
        /// </summary>
        internal readonly Dictionary<string, FileScope<TClass>> TypeUsing = new();
        public required Func<ClassDeclarationSyntax, FileScope<TClass>, bool> WhetherDeclared { get; init; }

        public required Func<ClassDeclarationSyntax, FileRegion, bool> Condition { get; init; }
        #endregion


        internal readonly Dictionary<string, FileRegion> Files = new();

        private FileRegion GetFile(string filePath)
        {
            if (Files.TryGetValue(filePath, out var ret)) return ret;
            ret = new() { FilePath = filePath };
            Files[filePath] = ret;
            return ret;
        }

        private FileScope<TClass> GetUsingSet(string filePath)
        {
            if (!TypeUsing.TryGetValue(filePath, out var namespaces))
            {
                namespaces = new() { FilePath = filePath };
                TypeUsing[filePath] = namespaces;
            }

            var res = TypeUsing.FirstOrDefault(x => x.Key != filePath && !x.Value.Used);
            if(res.Key != default){
                TypeUsing.Remove(res.Key);
            }
            return namespaces;
        }


        /// <summary>
        /// 解析using引用
        /// </summary>
        /// <param name="node"></param>
        internal void ResolveUsing(SyntaxNode node)
        {
            if (node is not UsingDirectiveSyntax realNode) return; //Using 声明
            if (!realNode.GetLocation().IsInSource) return;
            if (realNode.Name is not QualifiedNameSyntax subNode) return;
            var str = subNode.GetFullName();

            var file = GetFile(realNode.FilePath());
            if (!str.MayUse(typeof(TClass))) return;
            file.UsingNamespace(realNode);

            var usingSet = GetUsingSet(realNode.FilePath());
            if (!usingSet.IsQualifiedDeclaration(str)) return;
            usingSet.RegisterAlias(str,
                realNode
                    .Alias?
                    .Name
                    .Identifier
                    .Text);
        }
        /// <summary>
        /// 解析命名空间
        /// </summary>
        /// <param name="node"></param>
        internal void ResolveNamespaceDeclare(SyntaxNode node)
        {
            if (node is not BaseNamespaceDeclarationSyntax realNode) return; //Namespace 声明
            if (!realNode.GetLocation().IsInSource) return;
            if (realNode.Name is not QualifiedNameSyntax subNode) return;

            var str = subNode.GetFullName();

            var file = GetFile(realNode.FilePath());
            file.DeclareNamespace(realNode);


            var usingSet = GetUsingSet(realNode.FilePath());
            if (!usingSet.HasSameNamespace(str)) return;
            usingSet.RegisterAlias(usingSet.FullName);
        }
        /// <summary>
        /// 解析类型引用
        /// </summary>
        /// <param name="node"></param>
        internal void ResolveClassDeclare(SyntaxNode node)
        {
            if (node is not ClassDeclarationSyntax declareNode) return;//Class 声明

            var file = GetFile(declareNode.FilePath());

            var attrs = declareNode.GetAllAttributeSyntax();

            var set = GetUsingSet(node.FilePath());
            if (!declareNode.Has(SyntaxKind.PartialKeyword) || //不是partial class
                !WhetherDeclared(declareNode, set)) //不含有指定注解
                return;
            var tuple = declareNode.GetFullClassName();
            set.Use(tuple.Item1, declareNode);
        }
    }
}
