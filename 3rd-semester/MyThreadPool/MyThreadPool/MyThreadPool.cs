// <copyright file="MyThreadPool.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyThreadPool;

using System.Collections.Concurrent;

/// <summary>
/// Represents a thread pool that can execute tasks concurrently.
/// </summary>
/// <param name="threads"> The number of threads in the thread pool. </param>
public class MyThreadPool
{
    private readonly Thread[] threads;
    private readonly ConcurrentQueue<Action> queue = [];
    private readonly SemaphoreSlim semaphore = new(0);
    private readonly CancellationTokenSource token = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MyThreadPool"/> class with the default number of threads (number of processors).
    /// </summary>
    public MyThreadPool()
    {
        this.threads = new Thread[Environment.ProcessorCount];
        this.StartThreads();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyThreadPool"/> class with the specified number of threads.
    /// </summary>
    /// <param name="numberOfThreads"> The number of threads in the thread pool. </param>
    public MyThreadPool(int numberOfThreads)
    {
        this.threads = new Thread[numberOfThreads];
        this.StartThreads();
    }

    /// <summary>
    /// Submits a task to the thread pool for execution.
    /// </summary>
    /// <typeparam name="TResult"> The type of the result produced by the task. </typeparam>
    /// <param name="lambda"> The task to be executed. </param>
    /// <returns> A task representing the submitted work. </returns>
    public IMyTask<TResult> Submit<TResult>(Func<TResult> lambda)
    {
        var task = new MyTask<TResult>(this, lambda);

        this.Enqueue(task.Run);

        return task;
    }

    /// <summary>
    /// Shuts down the thread pool. Waits for running tasks to complete only.
    /// </summary>
    public void Shutdown()
    {
        this.token.Cancel();

        for (int i = 0; i < this.threads.Length; i++)
        {
            this.semaphore.Release();
        }

        foreach (var thread in this.threads)
        {
            thread.Join();
        }
    }

    /// <summary>
    /// Enqueues an action to be executed by the thread pool.
    /// </summary>
    /// <param name="action"> The action to be executed. </param>
    internal void Enqueue(Action action)
    {
        this.queue.Enqueue(action);
        this.semaphore.Release();
    }

    private void Worker()
    {
        Action? task = null;
        while (!this.token.IsCancellationRequested)
        {
            if (this.queue.TryDequeue(out task))
            {
                task();
                continue;
            }

            try
            {
                this.semaphore.Wait(this.token.Token);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    private void StartThreads()
    {
        for (int i = 0; i < this.threads.Length; i++)
        {
            this.threads[i] = new Thread(this.Worker) { IsBackground = true };
            this.threads[i].Start();
        }
    }
}
