using System;

// ReSharper disable once CheckNamespace
namespace Feast.JsonComment;

/// <summary>
/// 生成Json注释
/// </summary>
[AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
public class JsonCommentAttribute : Attribute { }