// <copyright file="ExpressionTree.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

module ExpressionTree

type Expression =
    | Number of int
    | Add of Expression * Expression
    | Subtract of Expression * Expression
    | Multiply of Expression * Expression
    | Divide of Expression * Expression

type ContinuationStep =
    | Finished of int option
    | Step of (unit -> ContinuationStep)

let rec linearize (expr: Expression) (cont: int option -> ContinuationStep) =
    let nextBinaryFunction left right nextFunction cont =
        Step(fun () ->
            linearize left (fun leftResult ->
                match leftResult with
                | Some leftValue ->
                    linearize right (fun rightResult ->
                        match rightResult with
                        | Some rightValue -> cont (nextFunction leftValue rightValue)
                        | None -> Finished None)
                | None -> Finished None))

    match expr with
    | Number x -> cont (Some x)
    | Add(leftExpr, rightExpr) -> nextBinaryFunction leftExpr rightExpr (fun left right -> Some(left + right)) cont
    | Subtract(leftExpr, rightExpr) -> nextBinaryFunction leftExpr rightExpr (fun left right -> Some(left - right)) cont
    | Multiply(leftExpr, rightExpr) -> nextBinaryFunction leftExpr rightExpr (fun left right -> Some(left * right)) cont
    | Divide(leftExpr, rightExpr) ->
        nextBinaryFunction leftExpr rightExpr (fun left right -> if right <> 0 then Some(left / right) else None) cont

let evaluateExpression (expr: Expression) =
    let steps = linearize expr (fun (res) -> Finished res)

    let rec processSteps step =
        match step with
        | Finished res -> res
        | Step next -> processSteps (next ())

    processSteps steps
