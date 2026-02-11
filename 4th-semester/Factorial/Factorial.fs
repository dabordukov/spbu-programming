let Factorial x =
    let rec factorial n acc =
        if n = 0 then acc else factorial (n - 1) (acc * n)

    if x < 0 then None else Some(factorial x 1)

printfn "Enter a number to calculate factorial of"
let numberString = stdin.ReadLine()
let number = int numberString

match Factorial number with
| Some result -> printfn "%d! = %d" number result
| None -> printfn "Error: number must not be negative"
