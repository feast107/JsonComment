using System;
using System.Collections.Generic;

namespace Feast.JsonAnnotation.Structs
{
    internal readonly struct FileScopeUsing
    {
        public FileScopeUsing(Type type)
        {
            Type = type;
            Namespace = type.Namespace ?? throw new ArgumentException($"Type {type} has no namespace");
            ClassName = type.IsSubclassOf(typeof(Attribute))
                ? type.Name.Replace(nameof(Attribute), string.Empty)
                : type.Name;
            FullName = $"{Namespace}.{ClassName}";
            aliasSet = new() { FullName };
        }
        public string Namespace { get; }
        public string ClassName { get; }
        public string FullName { get; }
        public Type Type { get; }
        private readonly HashSet<string> aliasSet;

        /// <summary>
        /// 是否有效声明
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public bool IsQualifiedDeclaration(string prefix) => prefix.Equals(Type.FullName) || FullName.StartsWith(prefix);

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
