// <copyright file="Program.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

string helpMessage = $"""
Console chat

    Usage: 
    Client mode: {System.AppDomain.CurrentDomain.FriendlyName} [address] [port]
    Server mode: {System.AppDomain.CurrentDomain.FriendlyName} [port]
""";

if (args.Length == 1)
{
    if (!int.TryParse(args[0], out int port))
    {
        Console.Error.WriteLine("Invalid port");
        return;
    }

    var server = new ConsoleChat.ConsoleChat(port);
    await server.Run();
}
else if (args.Length == 2)
{
    if (!int.TryParse(args[1], out int port))
    {
        Console.Error.WriteLine("Invalid port");
        return;
    }

    var client = new ConsoleChat.ConsoleChat(args[0], port);
    await client.Run();
}
else
{
    Console.WriteLine(helpMessage);
}
