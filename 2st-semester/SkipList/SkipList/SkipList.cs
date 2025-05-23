// <copyright file="SkipList.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SkipList;

using System.Collections;
using System.Transactions;

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
    /// Gets the generation of the skip list.
    /// </summary>
    public uint GetGeneration
        => this.generation;

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
    public T this[int index]
    {
        get
        {
            if (index < 0)
            {
                throw new InvalidOperationException();
            }

            Node? current = this.head[0];
            while (index-- > 0)
            {
                if (current is null)
                {
                    throw new ArgumentOutOfRangeException();
                }

                current = current.Next[0];
            }

            if (current is null)
            {
                throw new ArgumentOutOfRangeException();
            }

            return current.Value;
        }
        set => throw new NotSupportedException();
    }

    /// <summary>
    /// Adds an element to the list.
    /// </summary>
    /// <param name="item">The element to add.</param>
    public void Add(T item)
    {
        var oldLevels = this.levels;
        var nodeLevel = this.generator.Next();
        var newNode = new Node(item, nodeLevel + 1);

        if (nodeLevel >= this.levels)
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

        for (int i = oldLevels; i < this.levels; i++)
        {
            update[i] = this.head[i]; // filling new higher levels
        }

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
                newNode.Next[i] = this.head[i];
                this.head[i] = newNode;
            }
            else
            {
                newNode.Next[i] = update[i]!.Next[i];
                update[i]!.Next[i] = newNode;
            }
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

    private class Enumerator(SkipList<T> list) : IEnumerator<T>
    {
        private readonly SkipList<T> list = list;
        private readonly uint generation = list.GetGeneration;
        private bool disposed = false;
        private bool reseted = true;
        private Node? current = null;

        /// <summary>
        /// Gets the current element in the enumerator.
        /// </summary>
        public T Current
        {
            get
            {
                if (this.disposed || this.generation != this.list.GetGeneration || this.current is null)
                {
                    throw new InvalidOperationException();
                }

                return this.current.Value;
            }
        }

        /// <summary>
        /// Gets the current element in the enumerator.
        /// </summary>
        object IEnumerator.Current => this.Current!;

        /// <summary>
        /// Disposes the enumerator.
        /// </summary>
        public void Dispose()
        {
            this.disposed = true;
        }

        /// <summary>
        /// Moves to the next element in the enumerator.
        /// </summary>
        /// <returns>True if the enumerator was successfully moved to the next element; otherwise, false.</returns>
        public bool MoveNext()
        {
            if (this.disposed || this.generation != this.list.GetGeneration)
            {
                throw new InvalidOperationException();
            }

            if (this.reseted)
            {
                this.current = this.list.head[0];
                this.reseted = false;
                return true;
            }

            if (this.current is null)
            {
                return false;
            }

            this.current = this.current.Next[0];
            this.reseted = false;
            return true;
        }

        /// <summary>
        /// Resets the enumerator to its initial position.
        /// </summary>
        public void Reset()
        {
            if (this.disposed)
            {
                throw new InvalidOperationException();
            }

            this.current = null;
            this.reseted = true;
        }
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
