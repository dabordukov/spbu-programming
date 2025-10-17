// <copyright file="Program.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using SimpleFTPServer;

string helpMessage = $"""
SimpleFTP Server

    Usage: {System.AppDomain.CurrentDomain.FriendlyName} [IP_ADDRESS=0.0.0.0] [PORT=2222]
    
""";

if (args.Contains("--help") || args.Contains("-h"))
{
    Console.WriteLine(helpMessage);
    return 0;
}

string address = "0.0.0.0";
int port = 2222;

if (args.Length > 0)
{
    address = args[0];
}

if (args.Length > 1)
{
    if (!int.TryParse(args[1], out port))
    {
        Console.WriteLine("Invalid port number");
    }
}

var ftp = new FTPServer(address, port);
await ftp.StartAsync();

return 0;
