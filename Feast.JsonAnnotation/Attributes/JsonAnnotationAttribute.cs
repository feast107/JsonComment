using System;

namespace Feast.JsonAnnotation.Attributes
{
    /// <summary>
    /// 生成Json注释
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class JsonAnnotationAttribute : Attribute { }
}
