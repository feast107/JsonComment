using System;
using System.Collections.Generic;

namespace Feast.JsonComment.Extensions
{
    internal static class LoopExtension
    {
        internal static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var t in list)
            {
                action(t);
            }
        }
    }
}
