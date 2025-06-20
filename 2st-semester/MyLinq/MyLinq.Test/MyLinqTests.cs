// <copyright file="MyLinqTests.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyLinq.Test;

public class MyLinqTests
{
    [Test]
    public void First10_PrimeNumbersSequenceIsCorrect()
    {
        var primes = new List<int> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };
        var sequence = MyLinq.GetPrimes();

        Assert.That(Enumerable.SequenceEqual(primes, sequence.Take(10)), Is.True);
    }

    [Test]
    public void From10To20_PrimeNumbersSequenceIsCorrect()
    {
        var primes = new List<int> { 31, 37, 41, 43, 47, 53, 59, 61, 67, 71 };
        var sequence = MyLinq.GetPrimes();

        Assert.That(Enumerable.SequenceEqual(primes, sequence.Skip(10).Take(10)), Is.True);
    }

    [Test]
    public void Take_NegativeNumberOfElements_ReturnsEmptySequence()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
        Assert.That(Enumerable.SequenceEqual([], numbers.Take(-4)), Is.True);
    }

    [Test]
    public void Skip_NegativeNumberOfElements_ReturnsSourceSequence()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
        foreach (var x in Enumerable.Skip(numbers, -2))
        {
            Console.WriteLine(1);
        }

        Assert.That(Enumerable.SequenceEqual(numbers, numbers.Skip(-4)), Is.True);
    }
}
