open NumberFinder.NumberFinder

printfn "Enter numbers list (separated with ' ', ',', ';'):"
let input = stdin.ReadLine()

let list =
    input.Split([| ' '; ','; ';' |], System.StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map int

printfn "Enter number to find index of:"
let numberToFind = stdin.ReadLine() |> int


match find list numberToFind with
| Some result -> printfn "%d" result
| None -> printfn "Not found"
