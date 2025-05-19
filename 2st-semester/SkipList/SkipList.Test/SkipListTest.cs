// <copyright file="SkipListTest.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SkipList.Test;

public class SkipListTest
{
    private readonly int[] test = [5, 2, 6, 12, -1, 6];

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
}
