module LambdaInterpreter.Test

open NUnit.Framework
open FsUnit
open LambdaInterpreter

let id = Abstraction("x", Variable "x")
let constantK = Abstraction("x", Abstraction("y", Variable "x"))

let buildDeepAbs n =
    let rec loop current n =
        if n = 0 then
            current
        else
            loop (Abstraction(sprintf "x%d" n, current)) (n - 1)

    loop (Variable "y") n

let buildDeepApp n =
    let rec loop current n =
        if n = 0 then
            current
        else
            loop (Application(current, Variable "z")) (n - 1)

    loop (Variable "y") n


[<SetUp>]
let Setup () = ()


[<Test>]
let ``getFreeVariablesCPS should not overflow stack`` () =
    let depth = 1_000_000

    let deepExpr = buildDeepApp depth

    let result = getFreeVariablesCPS deepExpr
    result.Count |> should equal 2

[<Test>]
let ``normalize_should not overflow stack`` () =
    let depth = 1_000_0

    let rec applyN term n =
        if n = 0 then
            term
        else
            applyN (Application(term, Variable "target")) (n - 1)

    let deepExpression = applyN (buildDeepAbs depth) depth


    normalize deepExpression |> should equal (Variable "y")

[<Test>]
let ``getFreeVariablesCPS should return correct set for simple variable`` () =
    Variable "x" |> getFreeVariablesCPS |> should equal (Set.singleton "x")

[<Test>]
let ``getFreeVariablesCPS should handle bound variables in abstraction`` () =
    Abstraction("x", Variable "x") |> getFreeVariablesCPS |> should be Empty

[<Test>]
let ``getFreeVariablesCPS should identify free variables in application`` () =
    Application(Variable "x", Variable "y")
    |> getFreeVariablesCPS
    |> should equal (Set.ofList [ "x"; "y" ])

[<Test>]
let ``substitute should replace free variable correctly`` () =
    let expr = Application(Variable "x", Variable "z")
    let replacement = Variable "y"

    substitute "x" replacement expr
    |> should equal (Application(Variable "y", Variable "z"))

[<Test>]
let ``substitute should perform alpha-conversion to avoid capture`` () =
    // (λx. y) x
    let expr = Abstraction("x", Variable "y")
    let replacement = Variable "x"

    let result = substitute "y" replacement expr

    match result with
    | Abstraction(newName, Variable "x") -> newName |> should not' (equal "x")
    | _ -> failwith "Expected abstraction with renamed variable"

[<Test>]
let ``normalize should reduce identity application`` () =
    let app = Application(id, Variable "y")

    normalize app |> should equal (Variable "y")

[<Test>]
let ``normalize should handle nested Redexes`` () =
    // (λx. λy. x) A B -> A
    let term = Application(Application(constantK, Variable "A"), Variable "B")

    normalize term |> should equal (Variable "A")

[<Test>]
let ``Omega combinator should reduce to itself in one step`` () =
    let omegaInner = Abstraction("x", Application(Variable "x", Variable "x"))
    let omega = Application(omegaInner, omegaInner)

    let result = betaReduce omega

    result |> should equal (Some omega)
