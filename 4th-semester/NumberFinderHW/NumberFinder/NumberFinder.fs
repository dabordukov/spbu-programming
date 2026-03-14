// <copyright file="NumberFinder.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

module NumberFinder.NumberFinder

// Finds the first occurance of number in list
let find list number =
    let rec findIndex tail index =
        match tail with
        | [] -> None
        | h :: t -> if h = number then Some(index) else findIndex t (index + 1)

    findIndex list 0
