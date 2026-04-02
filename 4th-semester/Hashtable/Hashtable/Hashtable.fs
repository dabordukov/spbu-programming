// <copyright file="Hashtable.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

module HashTable

/// <summary>
/// Represents a hash table for storing unique integer values.
/// </summary>
/// <param name="size">The number of buckets in the hash table.</param>
/// <param name="hashFunction">A function used to compute a hash code for each item.</param>
type HashTable(size, hashFunction: int -> int) =
    let buckets = Array.create size []
    let mutable count = 0

    let getIndex item =
        let hash = hashFunction item
        abs hash % size

    /// <summary>
    /// Adds an item to the hash table if it is not already present.
    /// </summary>
    /// <param name="item">The item to add.</param>
    member h.Add(item) =
        let index = getIndex item

        if not (List.contains item buckets[index]) then
            buckets[index] <- item :: buckets[index]
            count <- count + 1

    /// <summary>
    /// Checks whether the hash table contains the specified item.
    /// </summary>
    /// <param name="item">The item to look for.</param>
    /// <returns><c>true</c> if the item exists in the table; otherwise <c>false</c>.</returns>
    member h.Contains(item) =
        let index = getIndex item
        List.contains item buckets[index]

    /// <summary>
    /// Removes the specified item from the hash table.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns><c>true</c> if the item was removed; otherwise <c>false</c>.</returns>
    member h.Remove(item) =
        let index = getIndex item
        let oldBucket = buckets[index]
        let newBucket = List.filter ((<>) item) oldBucket

        if List.length oldBucket <> List.length newBucket then
            buckets[index] <- newBucket
            count <- count - 1
            true
        else
            false

/// <summary>
/// Creates a hash table when the specified size is valid.
/// </summary>
/// <param name="size">The number of buckets to allocate.</param>
/// <param name="hashFunction">A function used to compute a hash code for each item.</param>
/// <returns>
/// <c>Some</c> hash table when <paramref name="size"/> is positive; otherwise <c>None</c>.
/// </returns>
let create size hashFunction =
    if size <= 0 then
        None
    else
        Some(HashTable(size, hashFunction))
