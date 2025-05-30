// <copyright file="FunctionsTest.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Functions.Test;

public class FunctionsTest
{
    [Test]
    public void Map_IntToString_ReturnsCorrectStrings()
    {
        var input = new List<int> { 1, 2, 3 };
        var result = Functions.Map(input, x => x.ToString());
        Assert.That(result, Is.EqualTo(new List<string> { "1", "2", "3" }));
    }

    [Test]
    public void Map_EmptyList_ReturnsEmptyList()
    {
        var input = new List<int>();
        var result = Functions.Map<int>(input, x => x * 2);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Map_DoubleNumbers_ReturnsCorrectList()
    {
        var input = new List<int> { 1, 2, 3 };
        var result = Functions.Map<int>(input, x => x * 2);
        Assert.That(result, Is.EqualTo(new List<int> { 2, 4, 6 }));
    }

    [Test]
    public void Filter_EvenNumbers_ReturnsOnlyEvenNumbers()
    {
        var input = new List<int> { 1, 2, 3, 4, 5, 6 };
        var result = Functions.Filter(input, x => x % 2 == 0);
        Assert.That(result, Is.EqualTo(new List<int> { 2, 4, 6 }));
    }

    [Test]
    public void Filter_EmptyList_ReturnsEmptyList()
    {
        var input = new List<int>();
        var result = Functions.Filter(input, x => x > 0);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Filter_NoMatchingElements_ReturnsEmptyList()
    {
        var input = new List<int> { 1, 3, 5 };
        var result = Functions.Filter(input, x => x % 2 == 0);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Fold_SumNumbers_ReturnsCorrectSum()
    {
        var input = new List<int> { 1, 2, 3, 4, 5 };
        var result = Functions.Fold<int>(input, 0, (acc, x) => acc + x);
        Assert.That(result, Is.EqualTo(15));
    }

    [Test]
    public void Fold_ConcatenateStrings_ReturnsCorrectString()
    {
        var input = new List<string> { "Hello", "World", "!" };
        var result = Functions.Fold<string>(input, string.Empty, (acc, x) => acc + x);
        Assert.That(result, Is.EqualTo("HelloWorld!"));
    }

    [Test]
    public void Fold_EmptyList_ReturnsAccumulator()
    {
        var input = new List<int>();
        var result = Functions.Fold<int>(input, 42, (acc, x) => acc + x);
        Assert.That(result, Is.EqualTo(42));
    }

    [Test]
    public void Fold_MultiplyNumbers_ReturnsCorrectProduct()
    {
        var input = new List<int> { 2, 3, 4, 5 };
        var result = Functions.Fold<int>(input, 1, (acc, x) => acc * x);
        Assert.That(result, Is.EqualTo(120));
    }
}
