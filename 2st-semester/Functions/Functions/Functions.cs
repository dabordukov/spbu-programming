namespace Functions;

public class Functions
{
    public static List<U> Map<T, U>(List<T> list, Func<T, U> function)
    {
        var newList = new List<U>(list.Count);
        foreach (var element in list)
        {
            newList.Add(function(element));
        }

        return newList;
    }

    public static List<T> Map<T>(List<T> list, Func<T, T> function)
    {
        return Map<T, T>(list, function);
    }

    public static List<T> Filter<T>(List<T> list, Func<T, bool> filter)
    {
        var newList = new List<T>();
        foreach (var element in list)
        {
            if (filter(element))
            {
                newList.Add(element);
            }
        }

        return newList;
    }

    public static U Fold<T, U>(List<T> list, U accumulator, Func<U, T, U> function)
    {
        foreach (var element in list)
        {
            accumulator = function(accumulator, element);
        }

        return accumulator;
    }

    public static T Fold<T>(List<T> list, T accumulator, Func<T, T, T> function)
    {
        return Fold<T, T>(list, accumulator, function);
    }
}
