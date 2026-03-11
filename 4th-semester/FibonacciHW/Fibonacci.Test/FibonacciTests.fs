module Fibonacci.Test

open NUnit.Framework
open FsUnit
open Fibonacci

[<TestCase(0, 0)>]
[<TestCase(1, 1)>]
[<TestCase(2, 1)>]
[<TestCase(8, 21)>]
[<TestCase(19, 4181)>]
let ``Should correctly calculate n-th Fibonacci number`` n expected =
    fibonacci n |> should equal (Some expected)

[<Test>]
let ``Should return None for n<0`` () = fibonacci -2 |> should equal None
