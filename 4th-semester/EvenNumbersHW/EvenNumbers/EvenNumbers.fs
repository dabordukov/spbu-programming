// <copyright file="EvenNumbers.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

module EvenNumbers

let countEvenNumbersMap list =
    List.map (fun x -> if x % 2 = 0 then 1 else 0) list |> List.sum

let countEvenNumbersFilter list =
    List.filter (fun x -> x % 2 = 0) list |> List.length

let countEvenNumbersFold list =
    List.fold (fun acc x -> if x % 2 = 0 then acc + 1 else acc) 0 list
