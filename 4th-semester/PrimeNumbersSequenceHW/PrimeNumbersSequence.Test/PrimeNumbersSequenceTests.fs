module PrimeNumbersSequence.Test

open NUnit.Framework
open FsUnit
open PrimeNumbers

[<Test>]
let ``GeneratePrimesSequence first numbers should be prime`` () =
    GeneratePrimesSequence
    |> Seq.take 5
    |> Seq.toList
    |> should equal [ 2; 3; 5; 7; 11 ]

let ``Prime number should be odd`` () =
    GeneratePrimesSequence
    |> Seq.skip 1
    |> Seq.take 100
    |> Seq.forall (fun n -> n % 2 <> 0)
    |> should be True
