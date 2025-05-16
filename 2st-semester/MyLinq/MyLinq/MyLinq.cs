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

            current += 2;
        }
    }

    /// <summary>
    /// This method takes the first n elements from the sequence.
    /// </summary>
    /// <param name="seq"> The original sequence.</param>
    /// <param name="n"> The number of elements to take.</param>
    /// <returns> The first n elements from the sequence.</returns>
    public static IEnumerable<int> Take(this IEnumerable<int> seq, int n)
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
    /// <param name="seq"> The original sequence.</param>
    /// <param name="n"> The number of elements to skip.</param>
    /// <returns> The sequence after skipping the first n elements.</returns>
    public static IEnumerable<int> Skip(this IEnumerable<int> seq, int n)
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
        int squareRoot = (int)Math.Sqrt(number);
        for (int i = 2; i <= squareRoot; i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }

        return true;
    }
}
