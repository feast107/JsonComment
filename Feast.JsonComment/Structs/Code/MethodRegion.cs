using System;
using Feast.JsonComment.Filters;

namespace Feast.JsonComment.Structs.Code
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
