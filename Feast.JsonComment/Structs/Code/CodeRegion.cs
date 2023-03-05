using Feast.JsonComment.Filters;

namespace Feast.JsonComment.Structs.Code
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
        where TFilter : SyntaxFilter<TFilter>
    {
        public static TFilter Filter { get; set; } 
        public static ProgramRegion<TFilter> Program { get; } = new();   
        public abstract string ContentString(int tab = 0);
        public virtual int DefaultTabCount { get; set; } = 1;

        public virtual bool Clip() => false;
    }
}
