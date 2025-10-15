// <copyright file="IMyTask.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyThreadPool;

/// <summary>
/// Represents a task that can be executed in a thread pool.
/// </summary>
/// <typeparam name="TResult"> The type of the result of the task. </typeparam>
public interface IMyTask<TResult>
{
    /// <summary>
    /// Gets a value indicating whether the task is completed.
    /// </summary>
    /// <returns> True if the task is completed, false otherwise. </returns>
    public bool IsCompleted { get; }

    /// <summary>
    /// Gets the result of the task. If the task is not completed, it waits for its completion.
    /// </summary>
    /// <returns> The result of the task. </returns>
    public TResult Result { get; }

    /// <summary>
    /// Creates a new task that will be executed after the current task is completed.
    /// </summary>
    /// <typeparam name="TNewResult"> The type of the result of the new task. </typeparam>
    /// <param name="nextTask"> The function that will be executed after the current task is completed. </param>
    /// <returns> The new task. </returns>
    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> nextTask);
}
