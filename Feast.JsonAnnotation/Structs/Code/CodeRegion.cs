using System;
using System.Collections.Generic;
using System.Text;
using Feast.JsonAnnotation.Filters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal abstract class CodeRegion
    {
        public enum TypeModifier
        {
            Class,
            Interface,
            Enum
        }

        public enum AccessModifier
        {
            Public,
            Protected,
            Private,
            Internal,
            File
        }

        public enum ExtraModifier
        {
            Extern,
            Readonly,
            Required,
            Abstract,
            Virtual,
            Static,
            Partial
        }
    }


    internal abstract class CodeRegion<TFilter> 
        where TFilter : ISyntaxFilter<TFilter>
    {
        public static TFilter Filter { get; set; } 
        public static ProgramRegion<TFilter> Program { get; } = new();   
        protected FileRegion<TFilter> Context(SyntaxNode node) => Program.GetContext(node);

        public abstract string ContentString(int tab = 0);
        public virtual int DefaultTabCount { get; set; } = 1;
    }
}
