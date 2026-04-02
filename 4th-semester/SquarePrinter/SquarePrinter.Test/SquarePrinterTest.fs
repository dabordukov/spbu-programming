module SquarePrinter.Test

open NUnit.Framework
open SquarePrinter
open FsUnit

[<Test>]
let ``square -1 should return empty string`` () =
    SquarePrinter.print -1 |> should equal ""

[<Test>]
let ``square 0 should return empty string`` () =
    SquarePrinter.print 0 |> should equal ""

[<Test>]
let ``square 1`` () =
    SquarePrinter.print 1 |> should equal "#\n"

[<Test>]
let ``square 2`` () =
    SquarePrinter.print 2 |> should equal "##\n##\n"

[<Test>]
let ``square 3`` () =
    SquarePrinter.print 3 |> should equal "###\n# #\n###\n"
