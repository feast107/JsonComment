using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Feast.JsonAnnotation.Structs;
using Feast.JsonAnnotation.Structs.Code;

namespace Feast.JsonAnnotation.Extensions
{
    internal static class FileGenerateExtension
    {
        public static string GenerateSourceFile<T>(this FileScope<T> scope)
        {
            var fileSource = new FileRegion();
            var ns = new NamespaceRegion() { Namespace = scope.Namespace };
            scope.UsingClass.ForEach(x =>
            {
            });
            throw new NotImplementedException();
        }
    }
}
