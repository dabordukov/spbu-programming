module ListReverse.Test

open NUnit.Framework
open FsUnit
open ListReverse.ListReverse

[<Test>]
let ``reverseList function must reverse list correctly`` () =
    reverseList [ 1; 2; 3; 4 ] |> should equal [ 4; 3; 2; 1 ]

[<Test>]
let ``reverseList function must return the same list on list of single element`` () =
    reverseList [ 1 ] |> should equal [ 1 ]

[<Test>]
let ``reverseList function must return empty list on empty list`` () = reverseList [] |> should equal []
