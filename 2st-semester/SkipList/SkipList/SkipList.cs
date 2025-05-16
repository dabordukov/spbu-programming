// <copyright file="SkipList.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SkipList;

using System.Collections;

/// <summary>
/// Implements a skip list.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class SkipList<T> : IList<T>
{
    private IComparer<T> comparer;
    private LevelGenerator generator = new LevelGenerator();
    private int size = 0;
    private int levels = 1;
    private Node?[] head = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="SkipList{T}"/> class.
    /// </summary>
    public SkipList()
    {
        this.comparer = Comparer<T>.Default;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SkipList{T}"/> class with a specified comparer.
    /// </summary>
    /// <param name="comparer"> The comparer to use for comparing elements.</param>
    public SkipList(IComparer<T> comparer)
    : this()
    {
        if (comparer is not null)
        {
            this.comparer = comparer;
        }
    }

    /// <summary>
    /// Gets the number of elements in the list.
    /// </summary>
    public int Count => this.size;

    /// <summary>
    /// Gets a value indicating whether the list is read-only.
    /// </summary>
    public bool IsReadOnly => true;

    /// <summary>
    /// Set or Gets element at the given index.
    /// </summary>
    /// <param name="index">The index of the element to get or set.</param>
    /// <returns>The value of the element to get or the value to set.</returns>
    public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Add(T item)
    {
        Node?[] update = new Node[this.levels];
        var next = this.head[this.levels - 1];
        for (int i = this.levels - 1; i >= 0; i--)
        {
            while (next is not null && this.comparer.Compare(item, next.Value) <= 0)
            {
                next = next.Next[i];
            }

            update[i] = next;
        }

        var newLevel = this.generator.Next();
        if (newLevel > this.levels - 1)
        {
            Array.Resize(ref this.head, newLevel);
            Array.Resize(ref update, newLevel);
            this.levels = newLevel + 1;
        }

        var newNode = new Node(item, newLevel + 1);

        for (int i = 0; i < this.levels; i++)
        {
            newNode.Next[i] = update[i]?.Next[i];
            if (update[i] is not null)
            {
                update[i].Next[i] = newNode;
            }
        }
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public int IndexOf(T item)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Inserts an element into the list at the specified index.
    /// </summary>
    /// <param name="index">The index at which to insert the element.</param>
    /// <param name="item">The element to insert.</param>
    /// <exception cref="NotSupportedException"> The list does not support insertion.</exception>
    public void Insert(int index, T item)
    {
        throw new NotSupportedException();
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class LevelGenerator : Random
    {
        public override int Next()
        {
            return -(int)Math.Ceiling(Math.Log2(this.Sample()));
        }
    }

    private class Node(T value, int levels)
    {
        public T Value { get; } = value;

        public Node?[] Next { get; set; } = new Node[levels];

        public int[] Length { get; set; } = new int[levels];
    }
}
