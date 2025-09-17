// <copyright file="ILazy.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Lazy;

/// <summary>
/// Defines a mechanism for lazy initialization.
/// </summary>
/// <typeparam name="T"> The type of object that is being lazily initialized.</typeparam>
public interface ILazy<T>
{
    /// <summary>
    /// Gets the lazily initialized value of the current <see cref="ILazy{T}"/> instance.
    /// </summary>
    /// <returns> The lazily initialized value.</returns>
    T Get();
}
