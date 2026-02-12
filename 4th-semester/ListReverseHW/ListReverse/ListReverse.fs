// <copyright file="ListReverse.fs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

module ListReverse.ListReverse

// Reverses the list
let reverseList list =
    let rec reverse newHead oldTail =
        match oldTail with
        | [] -> newHead
        | h :: t -> reverse (h :: newHead) t

    reverse [] list
