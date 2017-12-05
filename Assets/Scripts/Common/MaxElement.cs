using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
    public static TSource MaxElement<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TResult> selector
    )
    {
        var value = source.Max(selector);
        return source.FirstOrDefault(c => selector(c).Equals(value));
    }
}