// <copyright file="Program.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq.Expressions;

string helpMessage = $"""
Evaluate expression tree

    Usage: {System.AppDomain.CurrentDomain.FriendlyName} [INPUT FILE]
""";

if (args.Length != 1)
{
    Console.WriteLine(helpMessage);
    return 1;
}

if (!File.Exists(args[0]))
{
    Console.WriteLine($"File not found: {args[0]}");
    return 2;
}

var tree = new ExpressionTree.ExpressionTree(args[0]);
Console.WriteLine("Expression tree:");
tree.Print();
Console.WriteLine($"Result: {tree.Evaluate()}");

return 0;
