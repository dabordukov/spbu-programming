module HashTableTests

open NUnit.Framework
open FsUnit
open HashTable

let hash x = x % 10

[<Test>]
let ``Create with invalid size return None`` () = create 0 hash |> should equal None

[<Test>]
let ``Element exsits after being add`` () =
    let table = create 10 hash |> Option.get
    table.Add 5
    table.Contains 5 |> should equal true

[<Test>]
let ``Contains returns false for missing element`` () =
    let table = create 10 hash |> Option.get
    table.Contains 42 |> should equal false

[<Test>]
let ``Remove method do removes element`` () =
    let table = create 10 hash |> Option.get
    table.Add 7
    table.Remove 7 |> should equal true
    table.Contains 7 |> should equal false

[<Test>]
let ``Remove non-existing element return false`` () =
    let table = create 10 hash |> Option.get
    table.Remove 100 |> should equal false
