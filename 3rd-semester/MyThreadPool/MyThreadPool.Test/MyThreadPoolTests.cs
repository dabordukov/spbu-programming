// <copyright file="MyThreadPoolTests.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyThreadPool.Test;

public class MyThreadPoolTests
{
    [Test]
    public void Submit_TaskIsExecuted()
    {
        var pool = new MyThreadPool();
        bool executed = false;
        var task = pool.Submit(() =>
        {
            executed = true;
            return 42;
        });

        _ = task.Result;

        Assert.That(executed, Is.True);
    }

    [Test]
    public void Submit_TaskReturnsCorrectResult()
    {
        var pool = new MyThreadPool();
        var task = pool.Submit(() => 5 + 7);
        int result = task.Result;

        Assert.That(result, Is.EqualTo(12));
    }

    [Test]
    public void Submit_MultipleTasks_AllExecuted()
    {
        var pool = new MyThreadPool(4);
        int counter = 0;
        int tasksCount = 10;
        object lockObj = new();
        var tasks = new IMyTask<int>[tasksCount];
        for (int i = 0; i < tasksCount; i++)
        {
            tasks[i] = pool.Submit(() =>
            {
                lock (lockObj)
                {
                    counter++;
                }

                return counter;
            });
        }

        for (int i = 0; i < tasksCount; i++)
        {
            _ = tasks[i].Result;
        }

        Assert.That(counter, Is.EqualTo(tasksCount));
    }

    [Test]
    public void Shutdown_StopsAllThreads()
    {
        var pool = new MyThreadPool(2);
        pool.Shutdown();

        Assert.DoesNotThrow(() => pool.Submit(() => 1));
    }

    [Test]
    public void Submit_TaskThrowsException_ExceptionPropagates()
    {
        var pool = new MyThreadPool();

        var task = pool.Submit<int>(() => throw new InvalidOperationException("Test"));

        Assert.Throws<InvalidOperationException>(() => _ = task.Result);
    }

    [Test]
    public void ThreadPool_NumberOfThreads_AtLeast()
    {
        var semaphore = new SemaphoreSlim(0);
        var mres = new ManualResetEventSlim();
        var pool = new MyThreadPool(Environment.ProcessorCount);

        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            pool.Submit(() =>
            {
                semaphore.Release();
                mres.Wait();
                return true;
            });
        }

        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            if (!semaphore.Wait(2000))
            {
                Assert.Fail();
            }
        }
    }
}
