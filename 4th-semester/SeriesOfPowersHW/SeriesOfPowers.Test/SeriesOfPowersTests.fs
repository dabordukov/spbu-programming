module SeriesOfPowers.Test

open NUnit.Framework
open FsUnit
open SeriesOfPowers

[<Test>]
let ``Should correctly generate series for n > 0`` () =
    getListOfPowersOfTwo 2 2 |> should equal [ 4.; 8.; 16. ]

[<Test>]
let ``Should correctly generate series for n < 0`` () =
    getListOfPowersOfTwo -2 4 |> should equal [ 0.25; 0.5; 1; 2; 4 ]

[<Test>]
let ``Should return empty list for m < 0`` () =
    getListOfPowersOfTwo 2 -2 |> should equal []
