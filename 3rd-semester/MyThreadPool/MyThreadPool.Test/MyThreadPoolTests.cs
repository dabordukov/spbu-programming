// <copyright file="MyThreadPoolTests.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyThreadPool.Test;

using System.Collections.Concurrent;

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
            tasks[i] = pool.Submit(() => Interlocked.Increment(ref counter));
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

        Assert.Throws<TaskCanceledException>(() => pool.Submit(() => 1));
    }

    [Test]
    public void Submit_TaskThrowsException_ExceptionPropagates()
    {
        var pool = new MyThreadPool();

        var task = pool.Submit<int>(() => throw new InvalidOperationException("Test"));

        Assert.Throws<AggregateException>(() => _ = task.Result);
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

    [Test]
    public void Shutdown_RaceWithSubmit_DoesNotStuck()
    {
        var pool = new MyThreadPool(2);
        var tasksCount = 100;

        var shutdownThread = new Thread(() => pool.Shutdown());
        shutdownThread.Start();

        for (int i = 0; i < tasksCount; i++)
        {
            try
            {
                var task = pool.Submit(() => 1);
                _ = task.Result;
            }
            catch (TaskCanceledException)
            {
            }
        }

        shutdownThread.Join();
    }

    [Test]
    public void ContinueWith_AfterShutdown_ThrowsTaskCanceledException()
    {
        var pool = new MyThreadPool(1);
        var task = pool.Submit(() => 42);

        _ = task.Result;
        pool.Shutdown();

        Assert.Throws<TaskCanceledException>(() => task.ContinueWith(res => res * 2));
    }

    [Test]
    public void ContinueWith_RaceWithShutdown_AllContinuationsMustExecute()
    {
        const int numContinuations = 100;
        var pool = new MyThreadPool(4);
        var barrier = new Barrier(numContinuations + 1);

        var baseTask = pool.Submit(() => 1);
        _ = baseTask.Result;

        var acceptedContinuations = new ConcurrentBag<IMyTask<int>>();
        var cancellationCaught = 0;

        var threads = new List<Thread>();
        for (int i = 0; i < numContinuations; i++)
        {
            var t = new Thread(() =>
            {
                barrier.SignalAndWait();
                try
                {
                    var cont = baseTask.ContinueWith(res => res * 2);
                    acceptedContinuations.Add(cont);
                }
                catch (TaskCanceledException)
                {
                    Interlocked.Increment(ref cancellationCaught);
                }
            });
            threads.Add(t);
            t.Start();
        }

        var shutdownThread = new Thread(() =>
        {
            barrier.SignalAndWait();
            pool.Shutdown();
        });

        shutdownThread.Start();
        foreach (var t in threads)
        {
            t.Join();
        }

        shutdownThread.Join();

        foreach (var cont in acceptedContinuations)
        {
            bool finished = Task.Run(() =>
            {
                Assert.That(cont.Result, Is.EqualTo(84));
            }).Wait(1000);

            Assert.That(finished, Is.True);
        }
    }

    [Test]
    public void Shutdown_RaceWithSubmit_NoTasksLost()
    {
        const int threadsCount = 4;
        var pool = new MyThreadPool(threadsCount);
        var barrier = new Barrier(threadsCount + 1);
        var results = new ConcurrentBag<IMyTask<int>>();
        var exceptions = new ConcurrentBag<Exception>();

        var threads = new List<Thread>();
        for (int i = 0; i < threadsCount; i++)
        {
            var t = new Thread(() =>
            {
                barrier.SignalAndWait();
                try
                {
                    var task = pool.Submit(() => 1);
                    results.Add(task);
                }
                catch (TaskCanceledException ex)
                {
                    exceptions.Add(ex);
                }
            });

            threads.Add(t);
            t.Start();
        }

        barrier.SignalAndWait();
        pool.Shutdown();

        foreach (var t in threads)
        {
            t.Join();
        }

        foreach (var task in results)
        {
            bool completed = Task.Run(() => { _ = task.Result; }).Wait(1000);

            Assert.That(completed, Is.True, "Задача была принята, но не была исполнена после Shutdown.");
        }

        Assert.That(results.Count + exceptions.Count, Is.EqualTo(threadsCount));
    }

    [Test]
    public void Shutdown_MultipleThreadsCallingShutdown_ShouldNotThrowOrHang()
    {
        const int shutdownThreadsCount = 10;
        var pool = new MyThreadPool(4);
        var barrier = new Barrier(shutdownThreadsCount);
        var exceptions = new ConcurrentBag<Exception>();

        var threads = new List<Thread>();
        for (int i = 0; i < shutdownThreadsCount; i++)
        {
            var t = new Thread(() =>
            {
                barrier.SignalAndWait();
                try
                {
                    pool.Shutdown();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            });

            threads.Add(t);
            t.Start();
        }

        foreach (var t in threads)
        {
            if (!t.Join(TimeSpan.FromSeconds(5)))
            {
                Assert.Fail("Shutdown blocked thread.");
            }
        }

        Assert.Multiple(() =>
        {
            Assert.That(exceptions, Is.Empty, $"{exceptions.FirstOrDefault()?.Message}");
            Assert.That(pool.IsTurnedOff, Is.True);
        });
    }

    [Test]
    public void ContinueWith_CascadeException_PropagatesThroughAllStages()
    {
        var pool = new MyThreadPool(2);

        var rootTask = pool.Submit<int>(() =>
        {
            throw new InvalidOperationException("Root fail");
        });

        var level1 = rootTask.ContinueWith(res => res + 10);
        var level2 = level1.ContinueWith(res => res + 10);

        var root = Assert.Throws<AggregateException>(() => _ = rootTask.Result);
        Assert.That(root.InnerException, Is.TypeOf<InvalidOperationException>());

        Assert.Throws<AggregateException>(() => _ = level1.Result);
        var exLevel2 = Assert.Throws<AggregateException>(() => _ = level2.Result);

        Assert.That(exLevel2.InnerException, Is.TypeOf<InvalidOperationException>());
    }
}
