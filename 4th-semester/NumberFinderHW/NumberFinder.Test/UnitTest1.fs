module NumberFinder.Test

open NUnit.Framework
open FsUnit
open NumberFinder.NumberFinder

[<Test>]
let ``Must return index of the number in the middle of the list`` () =
    find [ 1; 2; 3; 3; 4; 5 ] 3 |> should equal (Some 2)

[<Test>]
let ``Must return index of the number in the end of the list`` () =
    find [ 1; 2; 3; 3; 4; 5 ] 5 |> should equal (Some 5)

[<Test>]
let ``Must return None if number is not in the list`` () =
    find [ 1; 2; 3; 3; 4; 5 ] 6 |> should equal None

[<Test>]
let ``Must return None on empty list`` () = find [] 6 |> should equal None
