// <copyright file="Program.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Net.Sockets;
using SimpleFTPClient;

string helpMessage = $"""
SimpleFTP Client

    Usage: {System.AppDomain.CurrentDomain.FriendlyName} [IP_ADDRESS] [PORT=2222]

""";

string menuMessage = """
Choose command:

    List directory: 1 <path>
    Get file:       2 <path>
    Exit:           <dot>/q/quit/exit/CTRL-D

Example:
1 ./
2 ../../../../../../../../../../../etc/shadow
.
""";

if (args.Length == 0 || args.Contains("--help") || args.Contains("-h"))
{
    Console.WriteLine(helpMessage);
    return 0;
}

string address = args[0];
int port = 2222;

if (args.Length > 1)
{
    if (!int.TryParse(args[1], out port))
    {
        Console.WriteLine("Invalid port number");
    }
}

try
{
    var tokenSource = new CancellationTokenSource();
    using var client = new FTPClient(address, port);

    Console.CancelKeyPress += (sender, eventArgs) =>
    {
        eventArgs.Cancel = true;
        tokenSource.Cancel();
    };

    try
    {
        await client.ConnectAsync(tokenSource.Token);
    }
    catch (TaskCanceledException)
    {
        Console.WriteLine("Connection cancelled by user.");
        return 0;
    }

    Console.WriteLine($"{menuMessage}\n");

    while (true)
    {
        var input = Console.ReadLine();
        if (input is null)
        {
            return 0;
        }

        var parts = input.Split(" ", 2);

        var command = parts[0].ToLower();

        if (command == "1")
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Invalid input");
                continue;
            }

            var list = client.List(parts[1]);
            if (list.List is null)
            {
                Console.WriteLine($"Error!");
                Console.WriteLine(list.Error);
                continue;
            }

            foreach (var pair in list.List)
            {
                if (pair.IsDirectory)
                {
                    Console.Write("[d] ");
                }
                else
                {
                    Console.Write("[f] ");
                }

                Console.WriteLine(pair.Name);
            }
        }
        else if (command == "2")
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Invalid input");
                continue;
            }

            Console.WriteLine("Enter path to file to save the downloading file into");
            var savePath = Console.ReadLine();
            if (savePath is null)
            {
                break;
            }

            var fileGet = client.Get(parts[1], savePath);

            if (fileGet.Error is not null)
            {
                Console.WriteLine($"Error!");
                Console.WriteLine(fileGet.Error);
                continue;
            }

            Console.WriteLine($"{fileGet.Size} bytes received successfully!");
        }
        else if (command is "." or "q" or "quit" or "exit")
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid input");
        }
    }
}
catch (SocketException e)
{
    Console.WriteLine("Can't connect to server.");
    Console.WriteLine(e.Message);
}

return 0;
