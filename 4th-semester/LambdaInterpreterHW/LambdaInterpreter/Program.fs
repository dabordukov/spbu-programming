module LambdaInterpreter

type Lambda =
    | Variable of string
    | Application of Lambda * Lambda
    | Abstraction of string * Lambda

type ContinuationStep<'a> =
    | Finished of 'a
    | Step of (unit -> ContinuationStep<'a>)

let runSteps step =
    let rec run s =
        match s with
        | Finished res -> res
        | Step next -> run (next ())

    run step

let getFreeVariablesCPS lambdaExpression =
    let rec linearize lambda cont =
        match lambda with
        | Variable x -> Step(fun () -> cont (Set.singleton x))
        | Application(left, right) ->
            Step(fun () ->
                linearize left (fun leftLambda ->
                    Step(fun () ->
                        linearize right (fun rightLambda -> Step(fun () -> cont (Set.union leftLambda rightLambda))))))
        | Abstraction(x, innerLambda) ->
            Step(fun () -> linearize innerLambda (fun result -> Step(fun () -> cont (Set.remove x result))))

    runSteps (linearize lambdaExpression (fun result -> Finished result))


let findFreshVar usedVars name =
    let rec loop i =
        let nextName = sprintf "%s_%d" name i

        if Set.contains nextName usedVars then
            loop (i + 1)
        else
            nextName

    loop 0

let substitute var value expr =
    let freeInValue = getFreeVariablesCPS value

    let rec substituteCPS variable value lambda cont =
        match lambda with
        | Variable x ->
            if x = variable then
                Step(fun () -> cont value)
            else
                Step(fun () -> cont (Variable x))

        | Application(left, right) ->
            Step(fun () ->
                substituteCPS variable value left (fun leftLambda ->
                    Step(fun () ->
                        substituteCPS variable value right (fun rightLambda ->
                            Step(fun () -> cont (Application(leftLambda, rightLambda)))))))

        | Abstraction(x, innerLambda) ->
            if x = variable then
                Step(fun () -> cont (Abstraction(x, innerLambda)))
            else if Set.contains x freeInValue then
                let freeInBody = getFreeVariablesCPS innerLambda

                if not (Set.contains variable freeInBody) then
                    Step(fun () -> cont (Abstraction(x, innerLambda)))
                else
                    let freshVar = findFreshVar (Set.union freeInValue freeInBody) x

                    Step(fun () ->
                        substituteCPS x (Variable freshVar) innerLambda (fun newBody ->
                            substituteCPS variable value (Abstraction(freshVar, newBody)) cont))
            else
                Step(fun () -> substituteCPS variable value innerLambda (fun body' -> cont (Abstraction(x, body'))))

    runSteps (substituteCPS var value expr Finished)

let rec betaReduceCPS expr cont =
    match expr with
    | Application(Abstraction(x, innerLambda), argument) ->
        Step(fun () -> cont (Some(substitute x argument innerLambda)))
    | Application(left, right) ->
        Step(fun () ->
            betaReduceCPS left (function
                | Some leftLambda -> Step(fun () -> cont (Some(Application(leftLambda, right))))
                | None ->
                    Step(fun () ->
                        betaReduceCPS right (function
                            | Some rightLambda -> cont (Some(Application(left, rightLambda)))
                            | None -> cont None))))
    | Abstraction(x, innerLambda) ->
        Step(fun () ->
            betaReduceCPS innerLambda (function
                | Some lambda -> cont (Some(Abstraction(x, lambda)))
                | None -> cont None))
    | Variable _ -> Step(fun () -> cont None)

let betaReduce expr = runSteps (betaReduceCPS expr Finished)

let rec normalizeCPS expr cont =
    Step(fun () ->
        betaReduceCPS expr (function
            | Some expr' -> normalizeCPS expr' cont
            | None -> cont expr))

let normalize expr = runSteps (normalizeCPS expr Finished)
