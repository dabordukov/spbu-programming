// <copyright file="MyList.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace NullCounter;

using System.Collections;

/// <summary>
/// Generic list.
/// </summary>
/// <typeparam name="T"> The type of the list elements. </typeparam>
public class MyList<T> : IEnumerable<T>
{
    private const int DefaultCapacity = 4;
    private T[] items;
    private int count;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyList{T}"/> class.
    /// </summary>
    public MyList()
    {
        this.items = new T[DefaultCapacity];
        this.count = 0;
    }

    /// <summary>
    /// Adds an item to the list.
    /// </summary>
    /// <param name="item"> The item to add. </param>
    public void Add(T item)
    {
        if (this.count == this.items.Length)
        {
            Array.Resize(ref this.items, this.items.Length * 2);
        }

        this.items[this.count++] = item;
    }

    /// <summary>
    /// Returns enumerator.
    /// </summary>
    /// <returns> Enumerator. </returns>
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < this.count; i++)
        {
            yield return this.items[i];
        }
    }

    /// <summary>
    /// Returns enumerator.
    /// </summary>
    /// <returns> Enumerator. </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
