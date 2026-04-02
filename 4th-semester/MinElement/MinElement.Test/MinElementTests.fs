module MinElement.Test

open NUnit.Framework
open FsUnit
open MinElement

[<Test>]
let ``Must return the min number in the middle of the list`` () =
    findMin [ 1; 2; 3; -5; 3; 4; 5 ] |> should equal (Some -5)

[<Test>]
let ``Must return the min number in the end of the list`` () =
    findMin [ 1; 2; 3; 3; 4; 5; 0 ] |> should equal (Some 0)

[<Test>]
let ``Must return the min number in the head of the list`` () =
    findMin [ 1; 2; 3; 3; 4; 5 ] |> should equal (Some 1)

[<Test>]
let ``Must return None if the list is empty`` () = findMin [] |> should equal None
