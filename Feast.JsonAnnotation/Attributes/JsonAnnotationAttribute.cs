using System;

// ReSharper disable once CheckNamespace
namespace Feast.JsonAnnotation;

/// <summary>
/// 生成Json注释
/// </summary>
[AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
public class JsonAnnotationAttribute : Attribute { }