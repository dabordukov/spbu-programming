open SquarePrinter

[<EntryPoint>]
let main argv =
    if argv.Length > 0 then
        printf "%s" (print (int argv[0]))
    else
        printfn "Pass the size of square as the 1st argument"

    0
