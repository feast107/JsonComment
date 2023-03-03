using System;
using System.Collections.Generic;
using System.Text;
using Feast.JsonAnnotation.Filters;

namespace Feast.JsonAnnotation.Structs.Code
{
    internal class MethodRegion<TFilter> : CodeRegion<TFilter>
        where TFilter : SyntaxFilter<TFilter>
    {
        public override string ContentString(int tab = 0)
        {
            throw new NotImplementedException();
        }
    }
}
