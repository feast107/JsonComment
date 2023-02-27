﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Feast.JsonAnnotation.Extensions
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
