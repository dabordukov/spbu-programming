// <copyright file="MinElement.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

module MinElement

/// <summary>
/// Finds the minimum element in a list.
/// </summary>
/// <param name="list">The list to search.</param>
/// <returns>
/// <c>Some</c> minimum value when the list is not empty; otherwise <c>None</c>.
/// </returns>
let findMin list =
    let rec find tail min =
        match tail with
        | [] -> min
        | h :: t -> find t (if h < min then h else min)

    match list with
    | [] -> None
    | h :: t -> Some(find t h)
