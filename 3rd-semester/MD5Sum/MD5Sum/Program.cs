// <copyright file="Program.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

string helpMessage = $"""
MD5Sum

    Usage: {System.AppDomain.CurrentDomain.FriendlyName} [path_to_directory]
""";

if (args.Length != 1)
{
    Console.WriteLine(helpMessage);
    return 1;
}

var normalPath = Path.GetFullPath(args[0]);
if (!Directory.Exists(args[0]))
{
    Console.WriteLine($"Directory not found: {args[0]}");
    return -1;
}

try
{
    var hashBytes = MD5Sum.MD5Sum.HashDirectory(args[0]);
    if (hashBytes is null)
    {
        Console.WriteLine("Can't calculate hash :(");
        return -2;
    }

    Console.WriteLine($"{hashBytes.ToString()}");
}
catch (ArgumentNullException e)
{
    Console.WriteLine($"Error: {e.Message}");
}

return 0;
