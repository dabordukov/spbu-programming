// <copyright file="INullChecker.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace NullCounter;

/// <summary>
/// This interface is used to check if an object is null.
/// </summary>
/// <typeparam name="T"> The type of the object to check.</typeparam>
public interface INullChecker<T>
{
    /// <summary>
    /// Checks if the object is null.
    /// </summary>
    /// <param name="o"> The object to check.</param>
    /// <returns> True if the object is null, false otherwise.</returns>
    bool IsNull(T o);
}
