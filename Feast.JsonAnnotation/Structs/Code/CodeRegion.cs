using System;
using System.Collections.Generic;
using System.Text;

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

        public abstract string ContentString(int tab = 0);

        public virtual int DefaultTabCount { get; set; } = 1;
    }
}
