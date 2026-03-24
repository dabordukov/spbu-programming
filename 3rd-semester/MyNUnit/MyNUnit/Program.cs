// <copyright file="Program.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using MyNUnit;

if (args.Length == 0)
{
    Console.WriteLine("Usage: MyNUnit <path_to_assemblies>");
    return;
}

var runner = new Runner();
await runner.RunTestsAsync(args[0]);

Console.WriteLine("Test report:");

foreach (var res in runner.Results)
{
    Console.WriteLine($"[{res.Status}] {res.ClassName}.{res.MethodName} ({res.DurationMs} ms)");

    if (!string.IsNullOrEmpty(res.Message))
    {
        Console.WriteLine($"\t{res.Message}");
    }
}
