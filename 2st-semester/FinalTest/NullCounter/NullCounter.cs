// <copyright file="NullCounter.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace NullCounter;

/// <summary>
/// This class is used to count the number of null elements in a list.
/// </summary>
public class NullCounter
{
    /// <summary>
    /// Counts the number of null elements in a list.
    /// </summary>
    /// <typeparam name="T">The type of list elements.</typeparam>
    /// <param name="list"> The list to check for null elements.</param>
    /// <param name="checker"> The null checker to use.</param>
    /// <returns>The number of null elements in the list.</returns>
    public static int CountNullElements<T>(MyList<T> list, INullChecker<T> checker)
    {
        ArgumentNullException.ThrowIfNull(list);
        ArgumentNullException.ThrowIfNull(checker);

        int count = 0;
        foreach (var x in list)
        {
            if (checker.IsNull(x))
            {
                count++;
            }
        }

        return count;
    }
}
