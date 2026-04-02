// <copyright file="LazyTests.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Lazy.Test;

public class LazyTests
{
    private static object[] lazyImplementationsInt =
    [
            new Func<Func<int>, ILazy<int>>(supplier => new LazySingleThread<int>(supplier)),
            new Func<Func<int>, ILazy<int>>(supplier => new LazyMultiThread<int>(supplier)),
    ];

    private static object[] lazyImplementationsObject =
    [
            new Func<Func<object>, ILazy<object>>(supplier => new LazySingleThread<object>(supplier)),
            new Func<Func<object>, ILazy<object>>(supplier => new LazyMultiThread<object>(supplier)),
    ];

    [TestCaseSource(nameof(lazyImplementationsInt))]
    public void Get_ReturnsSupplierValue(Func<Func<int>, ILazy<int>> lazyFunction)
    {
        var lazy = lazyFunction(() => 123);
        Assert.That(lazy.Get(), Is.EqualTo(123));
    }

    [TestCaseSource(nameof(lazyImplementationsInt))]
    public void Supplier_IsCalledOnlyOnce(Func<Func<int>, ILazy<int>> lazyFunction)
    {
        int callCount = 0;
        var lazy = lazyFunction(() =>
        {
            callCount++;
            return 99;
        });

        var first = lazy.Get();
        var second = lazy.Get();

        Assert.That(first, Is.EqualTo(99));
        Assert.That(second, Is.EqualTo(99));
        Assert.That(callCount, Is.EqualTo(1));
    }

    [TestCaseSource(nameof(lazyImplementationsInt))]
    public void Constructor_NullSupplier_ThrowsArgumentNullException(Func<Func<int>, ILazy<int>> lazyFunction)
    {
        Assert.Throws<ArgumentNullException>(() => lazyFunction(null!));
    }

    [Test]
    public void LazyMultiThread_IsThreadSafe_SupplierCalledOnce()
    {
        int callCount = 0;
        var lazy = new LazyMultiThread<int>(() =>
        {
            Interlocked.Increment(ref callCount);
            Thread.Sleep(50);
            return 1234;
        });

        int[] results = new int[10];
        Parallel.For(0, 10, i =>
        {
            results[i] = lazy.Get();
        });

        foreach (var result in results)
        {
            Assert.That(result, Is.EqualTo(1234));
        }

        Assert.That(callCount, Is.EqualTo(1));
    }

    [TestCaseSource(nameof(lazyImplementationsObject))]
    public void Get_ReturnsSameReferenceOnMultipleCalls(Func<Func<object>, ILazy<object>> lazyFunction)
    {
        var lazy = lazyFunction(() => new object());
        var first = lazy.Get();
        var second = lazy.Get();
        Assert.That(first, Is.EqualTo(second));
    }
}
