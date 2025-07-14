// <copyright file="NullCounterTest.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace NullCounter.Test;

public class NullCounterTest
{
    [Test]
    public void CountNullElements_ListWithEmptyStrings_ReturnsCorrectCount()
    {
        var list = new MyList<string>
        {
            "a",
            string.Empty,
            "b",
            string.Empty,
            "c",
        };
        Assert.That(NullCounter.CountNullElements(list, new StringNullChecker()), Is.EqualTo(2));
    }

    [Test]
    public void CountNullElements_EmptyList_ReturnsZero()
    {
        var list = new MyList<string>();
        Assert.That(NullCounter.CountNullElements(list, new StringNullChecker()), Is.EqualTo(0));
    }

    [Test]
    public void CountNullElements_NoNulls_ReturnsZero()
    {
        var list = new MyList<string> { "a", "b", "c" };
        Assert.That(NullCounter.CountNullElements(list, new StringNullChecker()), Is.EqualTo(0));
    }

    [Test]
    public void CountNullElements_NullList_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            NullCounter.CountNullElements(null, new StringNullChecker()));
    }

    [Test]
    public void CountNullElements_NullChecker_ThrowsArgumentNullException()
    {
        var list = new MyList<string>();
        Assert.Throws<ArgumentNullException>(() =>
            NullCounter.CountNullElements(list, null));
    }
}
