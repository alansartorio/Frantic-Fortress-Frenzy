using System;
using System.Collections.Generic;
using System.Linq;

public static class Extensions {
    
    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach (T item in enumeration)
        {
            action(item);
        }
    }
    
}

public static class UtilityEnumerable
{
    public static IEnumerable<T> Once<T>(T value)
    {
        return Enumerable.Repeat(value, 1);
    }
}
