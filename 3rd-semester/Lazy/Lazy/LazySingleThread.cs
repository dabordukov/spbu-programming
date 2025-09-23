// <copyright file="LazySingleThread.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Lazy;

/// <summary>
/// Provides support for lazy initialization. This class is not thread-safe.
/// </summary>
/// <typeparam name="T"> The type of object that is being lazily initialized.</typeparam>
/// <param name="supplier"> The delegate that is invoked to produce the lazily initialized value when it is needed.</param>
public class LazySingleThread<T>(Func<T> supplier) : ILazy<T>
{
    private Func<T>? supplier = supplier ?? throw new ArgumentNullException();
    private bool isInitialized = false;
    private T? value = default;

    /// <inheritdoc/>
    public T Get()
    {
        if (!this.isInitialized)
        {
            if (this.supplier is null)
            {
                throw new ArgumentNullException();
            }

            this.value = this.supplier();
            this.supplier = null;
            this.isInitialized = true;
        }

        return this.value!;
    }
}
