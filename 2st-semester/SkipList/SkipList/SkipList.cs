// <copyright file="SkipList.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SkipList;

using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

/// <summary>
/// Implements a skip list.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class SkipList<T> : IList<T>
{
    private static readonly int MaxLevels = 48;
    private IComparer<T> comparer;
    private LevelGenerator generator = new(MaxLevels);
    private int size = 0;
    private int levels = 1;
    private Node?[] head = new Node[1];
    private uint generation = 0;

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
    public bool IsReadOnly => false;

    /// <summary>
    /// Set or Gets element at the given index.
    /// </summary>
    /// <param name="index">The index of the element to get or set.</param>
    /// <returns>The value of the element to get or the value to set.</returns>
    public T this[int index] { get => throw new NotImplementedException(); set => throw new NotSupportedException(); }

    public void Add(T item)
    {
        var oldLevels = this.levels;
        var nodeLevel = this.generator.Next();
        var newNode = new Node(item, nodeLevel + 1);

        if (nodeLevel > this.levels)
        {
            Array.Resize(ref this.head, nodeLevel + 1);
            this.levels = nodeLevel + 1;
        }

        var update = new Node?[this.levels];
        var current = this.head[oldLevels - 1];
        if (current is null) // only if size == 0
        {
            for (int i = 0; i < this.levels; i++)
            {
                this.head[i] = newNode;
            }

            this.generation++;
            this.size++;
            return;
        }

        // for (int i = oldLevels; i < this.levels; i++)
        // {
        //     update[i] = this.head[i]; // filling new higher levels
        // }

        for (int i = this.levels - 1; i >= 0; i--)
        {
            while (current is not null)
            {
                if (this.comparer.Compare(current.Value, item) < 0)
                {
                    update[i] = current;
                    if (current.Next[i] is null)
                    {
                        break;
                    }

                    current = current.Next[i];
                }
                else
                {
                    current = update[i];
                    break;
                }
            }
        }

        for (int i = 0; i < newNode.Levels; i++)
        {
            if (update[i] is null)
            {
                throw new InvalidDataException();
            }

            newNode.Next[i] = update[i]?.Next[i];
            update[i] = newNode;
        }

        this.generation++;
        this.size++;
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

    private class LevelGenerator(int maxLevels) : Random
    {
        private readonly int maxLevels = maxLevels;

        public override int Next()
        {
            var value = Math.Log2(this.Sample());
            if (-value >= this.maxLevels)
            {
                return this.maxLevels;
            }

            return -(int)value;
        }
    }

    private class Node(T value, int levels)
    {
        public T Value { get; } = value;

        public Node?[] Next { get; set; } = new Node[levels];

        public int Levels { get; } = levels;
    }
}
