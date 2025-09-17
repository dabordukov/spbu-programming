// <copyright file="LazyTests.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Lazy.Test;

public class LazyTests
{
    private const int N = 1000;

    private static (ILazy<int>, int)[] lazy =
    {
        (new LazySingleThread<int>(Supplier), 1),
        (new LazyMultiThread<int>(Supplier), Environment.ProcessorCount),
    };

    [TestCaseSource(nameof(lazy))]
    public void Lazy_ManyRequest_ReturnsValue((ILazy<int> Lazy, int NumberOfThreads) args)
    {
        var results = new int[args.NumberOfThreads];
        var threads = new Thread[args.NumberOfThreads];
        for (int i = 0; i < args.NumberOfThreads; i++)
        {
            var index = i;
            threads[i] = new Thread(() => Simulate(results, index, args.Lazy));
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        foreach (var result in results)
        {
            Assert.That(result, Is.EqualTo(N));
        }
    }

    private static void Simulate(int[] results, int index, ILazy<int> lazy)
    {
        for (int i = 0; i < N; i++)
        {
            results[index] += lazy.Get();
        }
    }

    private static int Supplier()
    {
        Thread.Sleep(50);
        return 1;
    }
}
