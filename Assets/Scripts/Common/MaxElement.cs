using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
    public static TSource MaxElement<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TResult> selector
    ) where TSource : class
    {
        if (source.ToArray().Length == 0) {
            return null;
        }
        var value = source.Max(selector);
        return source.FirstOrDefault(c => selector(c).Equals(value));
    }
}