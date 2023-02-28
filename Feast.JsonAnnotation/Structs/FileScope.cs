using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;

namespace Feast.JsonAnnotation.Structs
{
    internal readonly struct FileScope<T>
    {
        #region Props
        public required string FilePath { get; init; }
        public string Directory => Path.GetDirectoryName(FilePath);

        public Type Type { get; } = typeof(T);

        public string ClassName { get; } =
            typeof(T).IsSubclassOf(typeof(Attribute))
                ? typeof(T).Name.Replace(nameof(Attribute), string.Empty)
                : typeof(T).Name;
        public string Namespace { get; } = typeof(T).Namespace ?? throw new ArgumentException($"Type {typeof(T)} has no namespace");
        public string FullName => $"{Namespace}.{ClassName}";

        public bool Used => UsingClass.Count > 0;
        #endregion

        public FileScope()
        {
            aliasSet = new() { FullName };
        }

        #region Fields

        private readonly HashSet<string> aliasSet;

        /// <summary>
        /// Key:命名空间,Value:类
        /// </summary>
        internal readonly Dictionary<string,HashSet<ClassDeclarationSyntax>> UsingClass = new();


        #endregion

        private HashSet<ClassDeclarationSyntax> GetClassesByNamespace(string nameSpace)
        {
            if (UsingClass.TryGetValue(nameSpace, out var set)) return set;
            set = new ();
            UsingClass[nameSpace] = set;
            return set;
        }

        /// <summary>
        /// 引用该类
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="node"></param>
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
