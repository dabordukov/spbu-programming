// <copyright file="ConsoleChat.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace ConsoleChat;

using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// Console chat class.
/// </summary>
public class ConsoleChat
{
    private readonly string address = string.Empty;
    private readonly int port;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleChat"/> class.
    /// </summary>
    /// <param name="address"> Address of the server to connect. </param>
    /// <param name="port"> Port of the server to connect. </param>
    /// <exception cref="ArgumentException"> Thrown when port is invalid. </exception>
    public ConsoleChat(string address, int port)
    : this(port)
    {
        this.address = address;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleChat"/> class.
    /// </summary>
    /// <param name="port"> Server port to listen. </param>
    /// <exception cref="ArgumentException"> Thrown when port is invalid. </exception>
    public ConsoleChat(int port)
    {
        if (port < 1 || port > 65535)
        {
            throw new ArgumentException($"Invalid port {port}");
        }

        this.port = port;
    }

    /// <summary>
    /// Runs the console chat.
    /// </summary>
    /// <returns> A <see cref="Task"/> representing the asynchronous chat task. </returns>
    public async Task Run()
    {
        if (this.address == string.Empty)
        {
            await this.RunServer();
        }
        else
        {
            await this.RunClient();
        }
    }

    private static async Task SendMessageLoop(NetworkStream stream, CancellationTokenSource token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                var message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                {
                    continue;
                }

                var data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length, token.Token);

                if (message == "exit")
                {
                    break;
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            token.Cancel();
        }
    }

    private static async Task ReadMessageLoop(NetworkStream stream, CancellationTokenSource token, string ip)
    {
        var buffer = new byte[4096];
        try
        {
            while (!token.IsCancellationRequested)
            {
                var readBytes = await stream.ReadAsync(buffer, 0, buffer.Length, token.Token);
                if (readBytes == 0)
                {
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, readBytes);
                Console.WriteLine($"[{ip}] {message}");

                if (message == "exit")
                {
                    break;
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            token.Cancel();
        }
    }

    private static async Task ClientHandler(TcpClient client)
    {
        var ip = "Unknown";

        var remote = client.Client.RemoteEndPoint;
        if (remote is not null)
        {
            ip = ((IPEndPoint)remote).Address.ToString();
        }

        using var stream = client.GetStream();
        using var token = new CancellationTokenSource();

        var readLoop = ReadMessageLoop(stream, token, ip);
        var sendLoop = SendMessageLoop(stream, token);

        await Task.WhenAny(readLoop, sendLoop);

        Console.Error.WriteLine($"{ip} disconnected");
    }

    private async Task RunServer()
    {
        var listener = new TcpListener(IPAddress.Any, this.port);
        listener.Start();
        Console.Error.WriteLine($"Server running on {this.port} port");

        var client = await listener.AcceptTcpClientAsync();

        await ClientHandler(client);

        listener.Stop();
    }

    private async Task RunClient()
    {
        if (this.address == string.Empty)
        {
            throw new ArgumentException("Address must not be empty for client mode");
        }

        using var client = new TcpClient();
        await client.ConnectAsync(this.address, this.port);
        await ClientHandler(client);
    }
}
