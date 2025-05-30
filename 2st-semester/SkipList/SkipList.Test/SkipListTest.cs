// <copyright file="SkipListTest.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SkipList.Test;

public class SkipListTest
{
    private static readonly int[][] RemoveTests = [[2, 5, 7], [5, 7, 9], [1, 2, 5]];
    private readonly int[] test = [5, 2, 6, 12, -1, 7];

    [Test]
    public void SkipListStoresElementsInAscendingOrder()
    {
        var list = new SkipList<int>();
        int[] expected = new int[this.test.Length];
        Array.Copy(this.test, expected, this.test.Length);
        Array.Sort(expected);

        foreach (var x in this.test)
        {
            list.Add(x);
        }

        Assert.That(Enumerable.SequenceEqual(list, expected), Is.True);
    }

    [Test]
    public void ClearRemovesAllElements()
    {
        var list = new SkipList<int>();
        foreach (var x in this.test)
        {
            list.Add(x);
        }

        list.Clear();
        Assert.That(list.Count, Is.EqualTo(0));
        Assert.That(list, Is.Empty);
    }

    [Test]
    public void ContainsReturnsTrueForExistingElement()
    {
        Assert.That(this.test.Contains(5), Is.True);
    }

    [Test]
    public void ContainsReturnsFalseForNonexistentElement()
    {
        var list = new SkipList<int>();
        list.Add(5);
        Assert.That(list.Contains(6), Is.False);
    }

    [Test]
    public void IndexOfReturnsCorrectIndex()
    {
        var list = new SkipList<int>();
        foreach (var x in this.test)
        {
            list.Add(x);
        }

        var sortedTest = this.test.OrderBy(x => x).ToArray();

        for (int i = 0; i < sortedTest.Length; i++)
        {
            Assert.That(list.IndexOf(sortedTest[i]), Is.EqualTo(i));
        }
    }

    [Test]
    public void IndexOfReturnsMinusOneForNonexistentElement()
    {
        var list = new SkipList<int>();
        foreach (var x in this.test)
        {
            list.Add(x);
        }

        Assert.That(list.IndexOf(100), Is.EqualTo(-1));
    }

    [TestCaseSource(nameof(RemoveTests))]
    public void RemoveReturnsTrueForExistingElement(IEnumerable<int> test)
    {
        SkipList<int> list = [.. test];
        int length = list.Count;
        Assert.That(list.Remove(5), Is.True);
        Assert.That(list.Count, Is.EqualTo(length - 1));
    }

    [Test]
    public void RemoveReturnsTrueForExistingAloneElement()
    {
        SkipList<int> list = [5];
        Assert.That(list.Remove(5), Is.True);
        Assert.That(list.Count, Is.EqualTo(0));
    }

    [Test]
    public void RemoveReturnsFalseForNonexistentElement()
    {
        var list = new SkipList<int>
        {
            5,
        };
        Assert.That(list.Remove(6), Is.False);
        Assert.That(list.Count, Is.EqualTo(1));
    }

    [Test]
    public void RemoveAtThrowsExceptionForInvalidIndex()
    {
        var list = new SkipList<int>();
        list.Add(5);
        Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(1));
    }

    [Test]
    public void RemoveAtRemovesCorrectElement()
    {
        var list = new SkipList<int>();
        foreach (var x in this.test)
        {
            list.Add(x);
        }

        var sortedTest = this.test.OrderBy(x => x).ToArray();
        list.RemoveAt(2);
        Assert.That(list.Contains(sortedTest[2]), Is.False);
        Assert.That(list.Count, Is.EqualTo(this.test.Length - 1));
    }

    [Test]
    public void IndexerThrowsExceptionForInvalidIndex()
    {
        var list = new SkipList<int>();
        list.Add(5);
        Assert.Throws<InvalidOperationException>(() => _ = list[-1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = list[1]);
    }

    [Test]
    public void IndexerReturnsCorrectElement()
    {
        var list = new SkipList<int>();
        foreach (var x in this.test)
        {
            list.Add(x);
        }

        var sortedTest = this.test.OrderBy(x => x).ToArray();
        for (int i = 0; i < sortedTest.Length; i++)
        {
            Assert.That(list[i], Is.EqualTo(sortedTest[i]));
        }
    }

    [Test]
    public void CopyToThrowsExceptionForNullArray()
    {
        var list = new SkipList<int>();
        Assert.Throws<ArgumentNullException>(() => list.CopyTo(null!, 0));
    }

    [Test]
    public void CopyToThrowsExceptionForNegativeIndex()
    {
        var list = new SkipList<int>();
        Assert.Throws<ArgumentOutOfRangeException>(() => list.CopyTo(new int[1], -1));
    }

    [Test]
    public void CopyToThrowsExceptionForInsufficientSpace()
    {
        var list = new SkipList<int>();
        list.Add(1);
        list.Add(2);
        Assert.Throws<ArgumentException>(() => list.CopyTo(new int[1], 0));
    }

    [Test]
    public void CopyToWorksCorrectly()
    {
        var list = new SkipList<int>();
        foreach (var x in this.test)
        {
            list.Add(x);
        }

        var array = new int[this.test.Length + 2];
        list.CopyTo(array, 1);

        var sortedTest = this.test.OrderBy(x => x).ToArray();
        for (int i = 0; i < sortedTest.Length; i++)
        {
            Assert.That(array[i + 1], Is.EqualTo(sortedTest[i]));
        }
    }

    [Test]
    public void EnumeratorThrowsExceptionAfterModification()
    {
        var list = new SkipList<int>();
        list.Add(1);
        var enumerator = list.GetEnumerator();
        list.Add(2);
        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }

    [Test]
    public void InsertThrowsNotSupportedException()
    {
        var list = new SkipList<int>();
        Assert.Throws<NotSupportedException>(() => list.Insert(0, 1));
    }

    [Test]
    public void SetterThrowsNotSupportedException()
    {
        var list = new SkipList<int>();
        list.Add(1);
        Assert.Throws<NotSupportedException>(() => list[0] = 2);
    }

    [Test]
    public void StressTest()
    {
        var n = 2000;
        var random = new Random();
        var list = new SkipList<int>();
        var elements = new int[n];
        for (int i = 0; i < n; i++)
        {
            elements[i] = random.Next(-50, 50);
            list.Add(elements[i]);
        }

        Assert.That(list.Count, Is.EqualTo(n));
        Assert.That(list.Contains(elements[random.Next(0, n)]), Is.True);
    }
}
