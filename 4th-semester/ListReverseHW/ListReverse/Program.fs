// <copyright file="Program.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

open ListReverse.ListReverse

printfn "Enter numbers list (separated with ' ', ',', ';'):"
let input = stdin.ReadLine()

let list =
    input.Split([| ' '; ','; ';' |], System.StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map int

printfn "%A" (reverseList list)
