// <copyright file="MyLinq.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyLinq;

/// <summary>
/// This class contains infinite sequence of prime numbers.
/// </summary>
public static class MyLinq
{
    /// <summary>
    /// This method generates an infinite sequence of prime numbers.
    /// </summary>
    /// <returns>An infinite sequence of prime numbers.</returns>
    public static IEnumerable<int> GetPrimes()
    {
        yield return 2;
        int current = 3;

        while (true)
        {
            if (IsPrime(current))
            {
                yield return current;
            }

            if (current > int.MaxValue - 2)
            {
                yield break;
            }

            current += 2;
        }
    }

    /// <summary>
    /// This method takes the first n elements from the sequence.
    /// </summary>
    /// <typeparam name="T"> The type of sequence elements.</typeparam>
    /// <param name="seq"> The original sequence.</param>
    /// <param name="n"> The number of elements to take.</param>
    /// <returns> The first n elements from the sequence.</returns>
    public static IEnumerable<T> Take<T>(this IEnumerable<T> seq, int n)
    {
        foreach (var next in seq)
        {
            if (n <= 0)
            {
                yield break;
            }

            yield return next;
            n--;
        }
    }

    /// <summary>
    /// This method skips the first n elements from the sequence.
    /// </summary>
    /// <typeparam name="T"> The type of sequence elements.</typeparam>
    /// <param name="seq"> The original sequence.</param>
    /// <param name="n"> The number of elements to skip.</param>
    /// <returns> The sequence after skipping the first n elements.</returns>
    public static IEnumerable<T> Skip<T>(this IEnumerable<T> seq, int n)
    {
        foreach (var next in seq)
        {
            if (n > 0)
            {
                n--;
            }
            else
            {
                yield return next;
            }
        }
    }

    private static bool IsPrime(int number)
    {
        if (number % 2 == 0)
        {
            return false;
        }

        int squareRoot = (int)Math.Sqrt(number);
        for (var i = 3; i <= squareRoot; i += 2)
        {
            if (number % i == 0)
            {
                return false;
            }
        }

        return true;
    }
}
