// <copyright file="Functions.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Functions;

/// <summary>
/// This class contains functional programming utilities.
/// </summary>
public class Functions
{
    /// <summary>
    /// Maps a function over a list and returns a new list.
    /// </summary>
    /// <typeparam name="TInput">Type of input objects.</typeparam>
    /// <typeparam name="TOutput">Type of output objects.</typeparam>
    /// <param name="list">List with input objects.</param>
    /// <param name="function">Function to apply to objects.</param>
    /// <returns>List of with function-applied objects.</returns>
    public static List<TOutput> Map<TInput, TOutput>(List<TInput> list, Func<TInput, TOutput> function)
    {
        var newList = new List<TOutput>(list.Count);
        foreach (var element in list)
        {
            newList.Add(function(element));
        }

        return newList;
    }

    /// <summary>
    /// Maps a function over a list and returns a new list.
    /// </summary>
    /// <typeparam name="T">Type of input objects.</typeparam>
    /// <param name="list">List with input objects.</param>
    /// <param name="function">Function to apply to objects.</param>
    /// <returns>List of with function-applied objects.</returns>
    public static List<T> Map<T>(List<T> list, Func<T, T> function)
    {
        return Map<T, T>(list, function);
    }

    /// <summary>
    /// Filters a list based on a predicate function.
    /// </summary>
    /// <typeparam name="T">Type of input objects.</typeparam>
    /// <param name="list">List with input objects.</param>
    /// <param name="filter">Predicate function to filter objects.</param>
    /// <returns>List of filtered objects for which the predicate function returned true.</returns>
    public static List<T> Filter<T>(List<T> list, Func<T, bool> filter)
    {
        var newList = new List<T>();
        foreach (var element in list)
        {
            if (filter(element))
            {
                newList.Add(element);
            }
        }

        return newList;
    }

    /// <summary>
    /// Reduces a list to a single value using a binary function.
    /// </summary>
    /// <typeparam name="TInput"> Type of input objects.</typeparam>
    /// <typeparam name="TOutput"> Type of output object.</typeparam>
    /// <param name="list"> List with input objects.</param>
    /// <param name="accumulator"> Initial value for the accumulator.</param>
    /// <param name="function"> Function to apply to objects.</param>
    /// <returns> Reduced value.</returns>
    public static TOutput Fold<TInput, TOutput>(List<TInput> list, TOutput accumulator, Func<TOutput, TInput, TOutput> function)
    {
        foreach (var element in list)
        {
            accumulator = function(accumulator, element);
        }

        return accumulator;
    }

    /// <summary>
    /// Reduces a list to a single value using a binary function.
    /// </summary>
    /// <typeparam name="T"> Type of input objects.</typeparam>
    /// <param name="list"> List with input objects.</param>
    /// <param name="accumulator"> Initial value for the accumulator.</param>
    /// <param name="function"> Function to apply to objects.</param>
    /// <returns> Reduced value.</returns>
    public static T Fold<T>(List<T> list, T accumulator, Func<T, T, T> function)
    {
        return Fold<T, T>(list, accumulator, function);
    }
}
