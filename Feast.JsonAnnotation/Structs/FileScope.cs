using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonAnnotation.Structs
{
    internal readonly struct FileScope
    {
        public FileScope(Type type)
        {
            Type = type;
            Namespace = type.Namespace ?? throw new ArgumentException($"Type {type} has no namespace");
            ClassName = type.IsSubclassOf(typeof(Attribute))
                ? type.Name.Replace(nameof(Attribute), string.Empty)
                : type.Name;
            FullName = $"{Namespace}.{ClassName}";
            aliasSet = new() { FullName };
            usingClass = new();
        }
        #region Fields
        public string ClassName { get; }
        public string Namespace { get; }
        public string FullName { get; }
        public Type Type { get; }
        private readonly HashSet<string> aliasSet;
        private readonly Dictionary<string,HashSet<ClassDeclarationSyntax>> usingClass;
        public bool Used => usingClass.Count > 0;
        #endregion
        private HashSet<ClassDeclarationSyntax> GetClassesByNamespace(string nameSpace)
        {
            if (usingClass.TryGetValue(nameSpace, out var set)) return set;
            set = new ();
            usingClass[nameSpace] = set;
            return set;
        }
        public void Use(string nameSpace, ClassDeclarationSyntax node) => GetClassesByNamespace(nameSpace).Add(node);
        /// <summary>
        /// 是否有效声明
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public bool IsQualifiedDeclaration(string prefix) => prefix.Equals(Type.FullName) || FullName.StartsWith(prefix);
        /// <summary>
        /// 是否来自一个命名空间
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public bool HasSameNamespace(string nameSpace) => nameSpace.StartsWith(Namespace);
#nullable enable
        /// <summary>
        /// 注册别名
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="alias"></param>
        public bool RegisterAlias(string baseName, string? alias = null) =>
            baseName.Equals(FullName) || baseName.Equals(Type.FullName)
                ? aliasSet.Add(alias ?? ClassName)
                : alias != null
                    ? aliasSet.Add($"{Namespace.Replace(baseName, alias)}.{ClassName}")
                    : aliasSet.Add(ClassName);
        /// <summary>
        /// 是否包含该声明
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public bool HasAttribute(string declaration) => aliasSet.Contains(declaration);
    }
}
