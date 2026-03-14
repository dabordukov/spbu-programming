// <copyright file="Program.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

open System
open SeriesOfPowers

let args = Environment.GetCommandLineArgs()

if args.Length = 3 then
    printfn "%A" (getListOfPowersOfTwo (int args.[1]) (int args.[2]))
else
    printfn "usage: ./bin n m\nn, m - integer numbers"
