module EvenNumbers.Test

open NUnit.Framework

open FsCheck.NUnit
open FsUnit
open EvenNumbers

[<Property>]
let ``Map is equivalent to filter`` (xs: int list) =
    countEvenNumbersMap xs = countEvenNumbersFilter xs

[<Property>]
let ``Filter is equivalent to fold`` (xs: int list) =
    countEvenNumbersFilter xs = countEvenNumbersFold xs

[<Property>]
let ``Fold is equivalent to map`` (xs: int list) =
    countEvenNumbersFold xs = countEvenNumbersMap xs

let testCase = [ 1; 2; 3; 4 ]
let testResult = 2

let testFunctions =
    [ countEvenNumbersMap; countEvenNumbersFilter; countEvenNumbersFold ]

[<TestCaseSource(nameof testFunctions)>]
let ``Function should count number of even numbers correctly`` (testFunction: int list -> int) =
    testFunction testCase |> should equal testResult
