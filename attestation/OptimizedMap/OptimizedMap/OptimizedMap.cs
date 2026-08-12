// <copyright file="OptimizedMap.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace OptimizedMap;

/// <summary>
/// Optimized map.
/// </summary>
/// <typeparam name="TKey">Key type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public class OptimizedMap<TKey, TValue>
where TKey : notnull
{
    private int size = 0;
    private object? storage = null;

    /// <summary>
    /// Gets size of map.
    /// </summary>
    public int Size => this.size;

    /// <summary>
    /// Adds new key:value pair if key is not in map. Otherwise updates value for given key.
    /// </summary>
    /// <param name="key">Key of pair.</param>
    /// <param name="value">Value of pair.</param>
    public void Put(TKey key, TValue value)
    {
        if (this.size == 0)
        {
            this.storage = new Node(key, value);
            this.size = 1;
            return;
        }

        if (this.size == 1)
        {
            Node node = this.storage as Node ?? throw new InvalidOperationException("Internal presentation is broken.");
            if (object.Equals(node.Key, key))
            {
                node.Value = value;
            }
            else
            {
                Node[] array = new Node[5];
                array[0] = node;
                array[1] = new Node(key, value);
                this.storage = array;
                this.size++;
            }

            return;
        }

        if (this.size <= 5)
        {
            Node[] array = this.storage as Node[] ?? throw new InvalidOperationException("Internal presentation is broken.");

            for (int i = 0; i < this.size; i++)
            {
                if (object.Equals(array[i].Key, key))
                {
                    array[i].Value = value;
                    return;
                }
            }

            if (this.size < 5)
            {
                array[this.size] = new Node(key, value);
                this.size++;
            }
            else
            {
                Dictionary<TKey, TValue> dict = [];
                for (int i = 0; i < 5; i++)
                {
                    dict[array[i].Key] = array[i].Value;
                }

                dict[key] = value;
                this.storage = dict;
                this.size++;
            }

            return;
        }

        Dictionary<TKey, TValue> map = this.storage as Dictionary<TKey, TValue> ?? throw new InvalidOperationException("Internal presentation is broken.");
        if (!map.ContainsKey(key))
        {
            this.size++;
        }

        map[key] = value;
    }

    /// <summary>
    /// Check if key exists in map.
    /// </summary>
    /// <param name="key">Key to check.</param>
    /// <returns>True if key exists, otherwise false.</returns>
    public bool ContainsKey(TKey key)
    {
        if (this.size == 0)
        {
            return false;
        }

        if (this.size == 1)
        {
            Node node = this.storage as Node ?? throw new InvalidOperationException("Internal presentation is broken.");
            return object.Equals(node.Key, key);
        }

        if (this.size <= 5)
        {
            Node[] array = this.storage as Node[] ?? throw new InvalidOperationException("Internal presentation is broken.");
            for (int i = 0; i < this.size; i++)
            {
                if (object.Equals(array[i].Key, key))
                {
                    return true;
                }
            }

            return false;
        }

        Dictionary<TKey, TValue> map = this.storage as Dictionary<TKey, TValue> ?? throw new InvalidOperationException("Internal presentation is broken.");
        return map.ContainsKey(key);
    }

    /// <summary>
    /// Gets value for given key.
    /// </summary>
    /// <param name="key">Key to get value by.</param>
    /// <returns>Value for given key.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the key is not found.</exception>
    public TValue Get(TKey key)
    {
        if (this.size == 0)
        {
            throw new KeyNotFoundException("Key is not found");
        }

        if (this.size == 1)
        {
            Node node = this.storage as Node ?? throw new InvalidOperationException("Internal presentation is broken.");
            if (object.Equals(node.Key, key))
            {
                return node.Value;
            }
        }
        else if (this.size <= 5)
        {
            Node[] array = this.storage as Node[] ?? throw new InvalidOperationException("Internal presentation is broken.");
            for (int i = 0; i < this.size; i++)
            {
                if (object.Equals(array[i].Key, key))
                {
                    return array[i].Value;
                }
            }
        }
        else
        {
            Dictionary<TKey, TValue> map = this.storage as Dictionary<TKey, TValue> ?? throw new InvalidOperationException("Internal presentation is broken.");
            if (map.ContainsKey(key))
            {
                return map[key];
            }
        }

        throw new KeyNotFoundException("Key is not found.");
    }

    private class Node(TKey key, TValue value)
    {
        public TKey Key { get; } = key;

        public TValue Value { get; set; } = value;
    }
}
