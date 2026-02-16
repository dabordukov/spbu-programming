// <copyright file="TreeMap.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

module TreeMap

type Tree =
    | Node of int * Tree * Tree
    | Empty

type ContinuationStep =
    | Finished of Tree
    | Step of (unit -> ContinuationStep)

let rec linearize (binTree: Tree) cont mapFunction =
    match binTree with
    | Empty -> Step(fun () -> cont Empty)
    | Node(value, left, right) ->
        Step(fun () ->
            linearize
                left
                (fun leftSubtree ->
                    linearize
                        right
                        (fun rightSubtree -> cont (Node(mapFunction value, leftSubtree, rightSubtree)))
                        mapFunction)
                mapFunction)

/// Creates a new tree by applying the function 'f' to each node's value.
let mapTree mapFunction binTree =
    let steps = linearize binTree (fun res -> Finished res) mapFunction

    let rec processSteps step =
        match step with
        | Finished tree -> tree
        | Step getNext -> processSteps (getNext ())

    processSteps steps
