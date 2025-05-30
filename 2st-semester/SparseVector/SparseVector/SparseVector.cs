// <copyright file="SparseVector.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SparseVector;

using System.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;

/// <summary>
/// This class represents a sparse vector.
/// </summary>
/// <typeparam name="T"> The type of the elements in the vector.</typeparam>
public class SparseVector<T>
where T : INumber<T>
{
    private readonly Dictionary<int, T> vector = [];
    private readonly int size;

    /// <summary>
    /// Initializes a new instance of the <see cref="SparseVector{T}"/> class.
    /// </summary>
    /// <param name="list"> The list of elements to initialize the sparse vector with.</param>
    /// <param name="checker"> The null checker to use for the elements.</param>
    public SparseVector(IEnumerable<T> list)
    {
        int count = 0;
        foreach (var x in list)
        {
            count++;
            this.vector.Add(this.vector.Count, x);
        }

        this.size = count;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SparseVector{T}"/> class with a specified size.
    /// </summary>
    /// <param name="size"> The size of the sparse vector.</param>
    public SparseVector(int size)
    {
        this.size = size;
    }

    /// <summary>
    /// Gets the size of vector.
    /// </summary>
    public int Size => this.size;

    public static SparseVector<T> operator +(SparseVector<T> a, SparseVector<T> b)
    {
        return OperatorInternals(a, b, '+');
    }

    public static SparseVector<T> operator -(SparseVector<T> a, SparseVector<T> b)
    {
        return OperatorInternals(a, b, '-');
    }

    public static SparseVector<T> operator *(SparseVector<T> a, SparseVector<T> b)
    {
        return OperatorInternals(a, b, '*');
    }

    /// <summary>
    /// Checks if the vector is null (all coordinates are zero).
    /// </summary>
    /// <returns> True if the vector is null, false otherwise.</returns>
    public bool IsNull() => this.vector.Count == 0;

    private static SparseVector<T> OperatorInternals(SparseVector<T> a, SparseVector<T> b, char operation)
    {
        if (a.Size != b.Size)
        {
            throw new InvalidOperationException("Vectors must be same size");
        }

        var newVector = new SparseVector<T>(a.Size);

        var aKeys = a.vector.GetEnumerator();
        var bKeys = b.vector.GetEnumerator();

        bool aNotEnd = aKeys.MoveNext();
        bool bNotEnd = bKeys.MoveNext();

        while (aNotEnd && bNotEnd)
        {
            if (aKeys.Current.Key == bKeys.Current.Key)
            {
                T newValue = DoOperation(aKeys.Current.Value, bKeys.Current.Value, operation);
                if (newValue != T.Zero)
                {
                    newVector.vector[aKeys.Current.Key] = newValue;
                }

                aNotEnd = aKeys.MoveNext();
                bNotEnd = bKeys.MoveNext();
            }
            else if (aKeys.Current.Key > bKeys.Current.Key)
            {
                T newValue = DoOperation(bKeys.Current.Value, a.vector[bKeys.Current.Key], operation);
                if (newValue != T.Zero)
                {
                    newVector.vector[bKeys.Current.Key] = newValue;
                }

                bNotEnd = bKeys.MoveNext();
            }
            else if (aKeys.Current.Key < bKeys.Current.Key)
            {
                T newValue = DoOperation(aKeys.Current.Value, b.vector[aKeys.Current.Key], operation);
                if (newValue != T.Zero)
                {
                    newVector.vector[aKeys.Current.Key] = newValue;
                }

                aNotEnd = aKeys.MoveNext();
            }
        }

        while (aNotEnd)
        {
            T newValue = DoOperation(aKeys.Current.Value, b.vector[aKeys.Current.Key], operation);
            if (newValue != T.Zero)
            {
                newVector.vector[aKeys.Current.Key] = newValue;
            }

            aNotEnd = aKeys.MoveNext();
        }

        while (bNotEnd)
        {
            T newValue = DoOperation(bKeys.Current.Value, a.vector[bKeys.Current.Key], operation);
            if (newValue != T.Zero)
            {
                newVector.vector[bKeys.Current.Key] = newValue;
            }

            bNotEnd = bKeys.MoveNext();
        }

        return newVector;
    }

    private static T DoOperation(T a, T b, char operation)
    {
        return operation switch
        {
            '+' => a + b,
            '-' => a - b,
            '*' => a * b,
            _ => T.Zero,
        };
    }
}
