// <copyright file="Fibonacci.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

module Fibonacci

let fibonacci n =
    let rec fib previous counter acc =
        if counter = 0 then
            Some(previous)
        else
            fib acc (counter - 1) (acc + previous)

    if n < 0 then None else fib 0 n 1
