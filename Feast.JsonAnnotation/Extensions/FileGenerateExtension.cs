using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Feast.JsonAnnotation.Structs;
using Feast.JsonAnnotation.Structs.Code;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonAnnotation.Extensions
{
    internal static class FileGenerateExtension
    {
        public static string GenerateSourceFile<T>(this FileScope<T> scope)
        {
            List<NamespaceRegion> namespaces = new();
            scope.UsingClass.ForEach(x =>
            {
                x.Value.ForEach(c =>
                {
                    var cr = new ClassRegion()
                    {
                        ClassName = c.GetSelfClassName(),
                        Class = c
                    };
                    var nsp = c.GetNamespace();
                    if (!namespaces.Any(n =>
                        {
                            if (!n.Namespace.Equals(nsp)) return false;
                            if (!n.Classes.Any(p =>
                                {
                                    if (!c.IsInnerClassOf(p.Class)) return false;
                                    p.Classes.Add(cr);
                                    return true;
                                }))
                            {
                                n.Classes.Add(cr);
                            };
                            return true;
                        }))
                    {
                        namespaces.Add(new ()
                        {
                            Namespace = nsp , Classes = { cr }
                        });
                    }
                    
                   
                });
            });
            return new FileRegion() { Namespaces = namespaces }.ContentString();
        }
    }
}
