using System;
using System.Collections.Generic;
using System.Xml;

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
        public string Namespace { get; }
        public string ClassName { get; }
        public string FullName { get; }
        public Type Type { get; }
        private readonly HashSet<string> aliasSet;
        private readonly Dictionary<string,HashSet<string>> usingClass;
        private HashSet<string> GetClassesByNamespace(string nameSpace)
        {
            if (usingClass.TryGetValue(nameSpace, out var set)) return set;
            set = new ();
            usingClass[nameSpace] = set;
            return set;
        }
        public bool Used => usingClass.Count > 0;
        public void Use(string nameSpace, string className) => GetClassesByNamespace(nameSpace).Add(className);

        /// <summary>
        /// 是否有效声明
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public bool IsQualifiedDeclaration(string prefix) => prefix.Equals(Type.FullName) || FullName.StartsWith(prefix);

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
