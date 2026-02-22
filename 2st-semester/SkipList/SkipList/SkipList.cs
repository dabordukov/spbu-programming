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
public class SkipList<T>(IComparer<T> comprarer) : IList<T>
{
    private static readonly int MaxLevels = 48;
    private IComparer<T> comparer = comprarer;
    private LevelGenerator generator = new(MaxLevels);
    private int size = 0;
    private int levels = 1;
    private Node?[] head = new Node[1];
    private uint generation = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="SkipList{T}"/> class with default comparer.
    /// </summary>
    /// <param name="comparer"> The comparer to use for comparing elements.</param>
    public SkipList()
    : this(Comparer<T>.Default)
    {
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

        for (int i = oldLevels - 1; i >= 0; i--)
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
                    if (i > 0)
                    {
                        current = this.head[i - 1];
                    }

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

    /// <summary>
    /// Removes all elements from the list.
    /// </summary>
    public void Clear()
    {
        this.head = new Node[1];
        this.levels = 1;
        this.size = 0;
        this.generation++;
    }

    /// <summary>
    /// Checks if the list contains a specific item.
    /// </summary>
    /// <param name="item"> The item to check for.</param>
    /// <returns> True if the item is found in the list; otherwise, false.</returns>
    public bool Contains(T item)
    {
        var current = this.head[this.levels - 1];

        for (int i = this.levels - 1; i >= 0; i--)
        {
            if (current != null && this.comparer.Compare(current.Value, item) > 0)
            {
                current = this.head[i];
            }

            while (current?.Next[i] != null && this.comparer.Compare(current.Next[i]!.Value, item) < 0)
            {
                current = current.Next[i];
            }
        }

        if (current is not null && this.comparer.Compare(current.Value, item) != 0)
        {
            current = current?.Next[0];
        }

        return current != null && this.comparer.Compare(current.Value, item) == 0;
    }

    /// <summary>
    /// Copies the elements of the list to an array, starting at a specified array index.
    /// </summary>
    /// <param name="array"> The array to copy the elements to.</param>
    /// <param name="arrayIndex"> The index at which copying begins.</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);

        ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);

        if (array.Length - arrayIndex < this.size)
        {
            throw new ArgumentException("Not enough space");
        }

        var current = this.head[0];
        var currentIndex = arrayIndex;

        while (current != null)
        {
            array[currentIndex] = current.Value;
            current = current.Next[0];
            currentIndex++;
        }
    }

    /// <summary>
    /// Gets an enumerator that iterates through the list.
    /// </summary>
    /// <returns> An enumerator for the list.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return new Enumerator(this);
    }

    /// <summary>
    /// Gets the index of the specified item in the list.
    /// </summary>
    /// <param name="item"> The item to find the index of.</param>
    /// <returns> The index of the item in the list, or -1 if the item is not found.</returns>
    public int IndexOf(T item)
    {
        var current = this.head[0];
        var index = 0;

        while (current != null)
        {
            if (this.comparer.Compare(current.Value, item) == 0)
            {
                return index;
            }

            current = current.Next[0];
            index++;
        }

        return -1;
    }

    /// <summary>
    /// Inserts an element into the list at the specified index.
    /// </summary>
    /// <param name="index">The index at which to insert the element.</param>
    /// <param name="item">The element to insert.</param>
    public void Insert(int index, T item) => throw new NotSupportedException();

    /// <summary>
    /// Removes the first occurrence of a specific item from the list.
    /// </summary>
    /// <param name="item"> The item to remove.</param>
    /// <returns> True if the item was successfully removed; otherwise, false.</returns>
    public bool Remove(T item)
    {
        var update = new Node?[this.levels];
        var current = this.head[this.levels - 1];

        Node? toDelete = null;

        for (int i = this.levels - 1; i >= 0; i--)
        {
            if (current != null && this.comparer.Compare(current.Value, item) > 0)
            {
                current = this.head[i];
            }

            while (current!.Next[i] != null && this.comparer.Compare(current.Next[i]!.Value, item) <= 0)
            {
                current = current.Next[i];
            }

            if (current != null && this.comparer.Compare(current.Value, item) == 0)
            {
                toDelete = current;
                break;
            }
        }

        if (toDelete is null)
        {
            return false;
        }

        current = this.head[toDelete.Levels - 1];
        for (int i = toDelete.Levels - 1; i >= 0; i--)
        {
            while (current != null && current.Next[i] != null && current != toDelete)
            {
                current = current.Next[i];
                update[i] = current;
            }
        }

        if (current is null)
        {
            return false;
        }

        for (int i = 0; i < toDelete.Levels; i++)
        {
            if (update[i] is null) // if the target was the first element in line
            {
                this.head[i] = this.head[i]?.Next[i];
            }
            else
            {
                update[i]!.Next[i] = toDelete.Next[i];
            }
        }

        while (this.levels > 1 && this.head[this.levels - 1] == null)
        {
            this.levels--;
        }

        this.size--;
        this.generation++;
        return true;
    }

    /// <summary>
    /// Removes the element at the specified index.
    /// </summary>
    /// <param name="index"> The index of the element to remove.</param>
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= this.size)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        var toDelete = this.head[0];
        while (index-- > 0)
        {
            toDelete = toDelete!.Next[0];
        }

        var current = this.head[toDelete!.Levels - 1];
        var update = new Node?[toDelete!.Levels];
        for (int i = toDelete.Levels - 1; i >= 0; i--)
        {
            if (current != null && this.comparer.Compare(current.Value, toDelete.Value) >= 0)
            {
                current = this.head[i];
            }

            while (current != null && current!.Next[i] != null && current.Next[i] != toDelete)
            {
                current = current.Next[i];
            }

            if (current != null && current.Next[i] == toDelete)
            {
                update[i] = current;
            }
        }

        for (int i = 0; i < toDelete.Levels; i++)
        {
            if (update[i] is null) // if the target was the first element in line
            {
                this.head[i] = this.head[i]?.Next[i];
            }
            else
            {
                update[i]!.Next[i] = toDelete.Next[i];
            }
        }

        while (this.levels > 1 && this.head[this.levels - 1] == null)
        {
            this.levels--;
        }

        this.size--;
        this.generation++;
    }

    /// <summary>
    /// Gets an enumerator that iterates through the list.
    /// </summary>
    /// <returns> An enumerator for the list.</returns>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

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
                if (this.current is null)
                {
                    this.reseted = true;
                    return false;
                }

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
