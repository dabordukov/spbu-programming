// <copyright file="PowerOfSeries.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

module SeriesOfPowers

// Generate [2^n; 2^(n + 1); ...; 2^(n + m)] list
let getListOfPowersOfTwo n m =
    let rec calculateFirstPower power acc =
        if power = 0 then
            acc
        else if power < 0 then
            calculateFirstPower (power + 1) (acc / 2.0)
        else
            calculateFirstPower (power - 1) (acc * 2.0)

    let rec generateList list counter =
        if counter < 1 then
            list
        else
            generateList (2.0 * List.head list :: list) (counter - 1)

    List.rev (generateList [ calculateFirstPower n 1.0 ] m)
