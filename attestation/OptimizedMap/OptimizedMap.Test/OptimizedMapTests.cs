// <copyright file="OptimizedMapTests.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace OptimizedMap.Test;

using OptimizedMap;

[TestFixture]
public class OptimizedMapTests
{
    private OptimizedMap<string, int> map;

    [SetUp]
    public void Setup()
    {
        this.map = new OptimizedMap<string, int>();
    }

    [Test]
    public void EmptyMapShouldThrowKeyNotFoundOnGet()
    {
        Assert.That(this.map.Size, Is.EqualTo(0));
        Assert.That(this.map.ContainsKey("key"), Is.False);
        Assert.Throws<KeyNotFoundException>(() => this.map.Get("key"));
    }

    [Test]
    public void OneElementShouldBeStoredCorrectly()
    {
        this.map.Put("1", 1);

        Assert.That(this.map.Size, Is.EqualTo(1));
        Assert.That(this.map.ContainsKey("1"), Is.True);
        Assert.That(this.map.Get("1"), Is.EqualTo(1));
    }

    [Test]
    public void PutShouldOverwriteExistingValue()
    {
        this.map.Put("1", 1);
        this.map.Put("1", 100);

        Assert.That(this.map.Size, Is.EqualTo(1));
        Assert.That(this.map.Get("1"), Is.EqualTo(100));
    }

    [Test]
    public void ArrayRepresentation_MultipleElementsShouldBeStoredCorrectly()
    {
        this.map.Put("1", 1);
        this.map.Put("2", 2);
        this.map.Put("3", 3);

        Assert.That(this.map.Size, Is.EqualTo(3));
        Assert.That(this.map.ContainsKey("2"), Is.True);
        Assert.That(this.map.Get("2"), Is.EqualTo(2));
        Assert.That(this.map.ContainsKey("3"), Is.True);
        Assert.That(this.map.Get("3"), Is.EqualTo(3));
    }

    [Test]
    public void DictionaryRepresentation_MultipleElementsShouldBeStoredCorrectly()
    {
        for (int i = 0; i < 10; i++)
        {
            this.map.Put($"{i}", i);
        }

        Assert.That(this.map.Size, Is.EqualTo(10));
        Assert.That(this.map.Get("4"), Is.EqualTo(4));
        Assert.That(this.map.Get("9"), Is.EqualTo(9));
    }
}
