module TreeMap.Tests

open NUnit.Framework
open FsUnit
open TreeMap

//      10
//     /  \
//    5    15
let testTree = Node(10, Node(5, Empty, Empty), Node(15, Empty, Empty))

[<TestCase(1, 11)>]
[<TestCase(10, 20)>]
[<TestCase(0, 10)>]
let ``mapTree should correctly transform root value`` increment expected =
    let newTree = mapTree (fun x -> x + increment) testTree

    match newTree with
    | Node(v, _, _) -> Some(v)
    | Empty -> None
    |> should equal (Some expected)

[<Test>]
let ``mapTree should handle Empty tree correctly`` () =
    mapTree (fun x -> x + 1) Empty |> should equal Empty

[<Test>]
let ``mapTree should not cause stack overflow`` () =
    let depth = 50000

    let rec build n acc =
        if n = 0 then acc else build (n - 1) (Node(n, acc, Empty))

    let deepTree = build depth Empty

    let rec getDepth (tree: Tree) =
        let rec loop t acc =
            match t with
            | Empty -> acc
            | Node(_, left, _) -> loop left (acc + 1)

        loop tree 0

    let result = mapTree (fun x -> x + 1) deepTree

    match result with
    | Node(v, _, _) -> Some(v)
    | _ -> None
    |> should equal (Some 2)

    getDepth deepTree |> should equal depth

[<Test>]
let ``mapTree should preserve tree structure`` () =
    let newTree = mapTree id testTree
    newTree |> should equal testTree
