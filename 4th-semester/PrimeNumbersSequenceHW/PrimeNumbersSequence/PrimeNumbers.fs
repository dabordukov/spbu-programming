// <copyright file="PrimeNumbers.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

module PrimeNumbers

// Checks if number is prime
let isPrime number =
    if number < 2 then
        false
    else
        let limit = int (sqrt (float number))

        let rec check x =
            if x > limit then true
            elif number % x = 0 then false
            else check (x + 1)

        check 2

// Generates infinite sequence of prime numbers
let GeneratePrimesSequence = Seq.initInfinite (fun i -> i) |> Seq.filter isPrime
