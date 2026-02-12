// <copyright file="MyTask.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyThreadPool;

using System.Collections.Concurrent;

/// <summary>
/// Represents a thread pool that can execute tasks concurrently.
/// </summary>
/// <param name="threads"> The number of threads in the thread pool. </param>
/// <typeparam name="TResult"> The type of the result produced by the task. </typeparam>
internal class MyTask<TResult>(MyThreadPool threadpool, Func<TResult> lambda) : IMyTask<TResult>
{
    private readonly Func<TResult> lambda = lambda;
    private readonly MyThreadPool threadpool = threadpool;
    private readonly ManualResetEventSlim eventLock = new(false);
    private readonly ConcurrentQueue<Action> nextTasks = [];
    private TResult? result;
    private Exception? exception;
    private bool ready;

    /// <inheritdoc/>
    public bool IsCompleted => this.ready;

    /// <inheritdoc/>
    public TResult Result
    {
        get
        {
            this.eventLock.Wait();
            if (this.exception is not null)
            {
                if (this.exception is AggregateException e)
                {
                    throw e;
                }

                throw new AggregateException(this.exception);
            }

            return this.result ?? throw new ArgumentNullException();
        }
    }

    /// <inheritdoc/>
    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> nextTask)
    {
        var newTask = new MyTask<TNewResult>(this.threadpool, () =>
        {
            return nextTask(this.Result);
        });

        lock (this.nextTasks)
        {
            if (this.IsCompleted)
            {
                this.threadpool.Enqueue(newTask.Run);
            }
            else
            {
                if (this.threadpool.IsTurnedOff)
                {
                    throw new TaskCanceledException("Threadpool is turned off");
                }

                this.nextTasks.Enqueue(newTask.Schedule);
            }
        }

        return newTask;
    }

    /// <summary>
    /// Schedules the task for execution in the thread pool.
    /// </summary>
    internal void Schedule()
        => this.threadpool.Enqueue(this.Run);

    /// <summary>
    /// Executes the task.
    /// </summary>
    internal void Run()
    {
        try
        {
            this.result = this.lambda();
        }
        catch (Exception e)
        {
            this.exception = e;
        }
        finally
        {
            this.ready = true;
            this.eventLock.Set();

            lock (this.nextTasks)
            {
                while (this.nextTasks.TryDequeue(out var task))
                {
                    try
                    {
                        task();
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }
            }
        }
    }
}
