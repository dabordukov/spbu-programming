// <copyright file="SquarePrinter.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

module SquarePrinter

/// <summary>
/// Prints square of size <c>n</c>.
/// </summary>
/// <param name="n">The size of square to print.</param>
let print n =

    let rec printLine chars char acc =
        if chars > 0 then
            printLine (chars - 1) char (acc + string char)
        else
            acc

    let rec printSquare lineNumber lines =
        if lineNumber > n then
            lines
        else
            let line =
                if lineNumber = n then printLine n '#' ""
                else if lineNumber = 1 then printLine n '#' ""
                else "#" + printLine (n - 2) ' ' "" + "#"

            printSquare (lineNumber + 1) (lines + line + "\n")

    if n > 0 then printSquare 1 "" else ""
