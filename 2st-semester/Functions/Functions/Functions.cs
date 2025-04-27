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
}
