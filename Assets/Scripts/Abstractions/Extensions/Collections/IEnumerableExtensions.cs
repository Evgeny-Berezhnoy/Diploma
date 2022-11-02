using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
    #region Methods

    public static T Random<T>(this IEnumerable<T> collection)
    {
        var count = collection.Count();

        if (count == 0) return default(T);

        var random  = new Random(Randomizer.Seed);
        var index   = random.Next(count);

        return collection.ElementAt(index);
    }

    public static Queue<T> ToQueue<T>(this IEnumerable<T> collection)
    {
        return new Queue<T>(collection);
    }

    #endregion
}