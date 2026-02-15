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
    match expr with
    | Number x -> cont (Some x)
    | Add(expr1, expr2) ->
        Step(fun () ->
            linearize expr1 (fun leftResult ->
                match leftResult with
                | Some leftValue ->
                    linearize expr2 (fun rightResult ->
                        match rightResult with
                        | Some rightValue -> cont (Some(leftValue + rightValue))
                        | None -> Finished None)
                | None -> Finished None))
    | Subtract(expr1, expr2) ->
        Step(fun () ->
            linearize expr1 (fun leftResult ->
                match leftResult with
                | Some leftValue ->
                    linearize expr2 (fun rightResult ->
                        match rightResult with
                        | Some rightValue -> cont (Some(leftValue - rightValue))
                        | None -> Finished None)
                | None -> Finished None))
    | Multiply(expr1, expr2) ->
        Step(fun () ->
            linearize expr1 (fun leftResult ->
                match leftResult with
                | Some leftValue ->
                    linearize expr2 (fun rightResult ->
                        match rightResult with
                        | Some rightValue -> cont (Some(leftValue * rightValue))
                        | None -> Finished None)
                | None -> Finished None))
    | Divide(expr1, expr2) ->
        Step(fun () ->
            linearize expr1 (fun leftResult ->
                match leftResult with
                | Some leftValue ->
                    linearize expr2 (fun rightResult ->
                        match rightResult with
                        | Some rightValue when rightValue <> 0 -> cont (Some(leftValue / rightValue))
                        | _ -> Finished None)
                | None -> Finished None))

let evaluateExpression (expr: Expression) =
    let steps = linearize expr (fun (res) -> Finished res)

    let rec processSteps step =
        match step with
        | Finished res -> res
        | Step next -> processSteps (next ())

    processSteps steps
