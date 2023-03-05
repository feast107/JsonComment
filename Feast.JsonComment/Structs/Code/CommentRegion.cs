using System.Collections.Generic;
using System.Text;
using Feast.JsonComment.Filters;

namespace Feast.JsonComment.Structs.Code
{
    internal class CommentRegion<TFilter> : CodeRegion<TFilter>
        where TFilter : SyntaxFilter<TFilter>
    {
        public List<string> Annotations = new();
        public override string ContentString(int tab = 0)
        {
            var sb = new StringBuilder();
            sb.AppendLine("/// <summary>");
            Annotations.ForEach(x => sb.AppendLine($"/// {x}"));
            sb.Append("/// </summary>");
            return sb.ToString();
        }
    }
}
