// <copyright file="FTPServer.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SimpleFTPServer;

using System.Net;
using System.Net.Sockets;
using System.Text;
using Logging;

/// <summary>
/// FTPServer class.
/// </summary>
public class FTPServer : IDisposable
{
    private const int ClientTimeoutSeconds = 60;
    private const long ErrorCode = -1;
    private readonly IPAddress ip;
    private readonly int port;
    private readonly TcpListener tcpListener;
    private readonly List<Task> activeTasks = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="FTPServer"/> class.
    /// </summary>
    /// <param name="ip"> IP address to listen on. </param>
    /// <param name="port"> Port to listen on. </param>
    public FTPServer(string ip, int port)
    {
        this.ip = IPAddress.Parse(ip);
        this.port = port;
        this.tcpListener = new TcpListener(this.ip, this.port);
    }

    /// <summary>
    /// Disposes the FTPServer.
    /// </summary>
    public void Dispose()
    {
        this.tcpListener.Dispose();
    }

    /// <summary>
    /// Starts the FTP server.
    /// </summary>
    /// <param name="token"> Cancellation token. </param>
    /// <returns> A <see cref="Task"/> representing the asynchronous operation of the server. </returns>
    public async Task StartAsync(CancellationToken token = default)
    {
        try
        {
            this.tcpListener.Start();
            Logging.Info($"Server started on {this.tcpListener.Server.LocalEndPoint}");
            while (!token.IsCancellationRequested)
            {
                var tcpClient = await this.tcpListener.AcceptTcpClientAsync(token);
                var task = Task.Run(
                async () =>
                {
                    try
                    {
                        using var connection = new TcpConnectionProcessor(tcpClient);
                        await connection.ProcessConnection();
                    }
                    catch (Exception e)
                    {
                        Logging.Error($"New exception: {e.Message}");
                    }
                },
                token);
                this.activeTasks.Add(task);
            }

            Task.WaitAll(this.activeTasks, token);
        }
        catch (Exception e)
        {
            Logging.Error($"New exception: {e.Message}");
            throw;
        }
        finally
        {
            this.tcpListener.Stop();
            Logging.Info("Server stopped");
        }
    }

    private class TcpConnectionProcessor : IDisposable
    {
        private const int BufferSize = 4096;
        private readonly TcpClient tcpClient;
        private readonly NetworkStream stream;
        private readonly StreamReader reader;
        private readonly BinaryWriter writer;
        private readonly string endpoint;

        public TcpConnectionProcessor(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.stream = tcpClient.GetStream();
            this.reader = new StreamReader(this.stream);
            this.writer = new BinaryWriter(this.stream);
            var remoteEndPoint = tcpClient.Client.RemoteEndPoint;
            string? endpointString;
            if (remoteEndPoint is null || (endpointString = remoteEndPoint.ToString()) is null)
            {
                throw new ArgumentNullException("tcpClient.Client.RemoteEndPoint", "Remote endpoint is not available");
            }

            this.endpoint = endpointString;
        }

        public void Dispose()
        {
            this.reader.Dispose();
            this.writer.Dispose();
            this.stream.Dispose();
            this.tcpClient.Dispose();
        }

        public async Task ProcessConnection()
        {
            Logging.Info("CONNECTED", this.endpoint);
            while (true)
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ClientTimeoutSeconds));
                string? line;
                try
                {
                    line = await this.reader.ReadLineAsync(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Logging.Info("TIMEOUT", this.endpoint);
                    this.SendError("Timeout");
                    break;
                }

                if (line is null)
                {
                    break;
                }

                var request = line.Split(' ', 2);
                if (request.Length != 2)
                {
                    this.SendError("Invalid request");
                }

                if (request[0] == "1")
                {
                    Logging.Info($"LIST {request[1]}", this.endpoint);
                    this.ListDirectory(request[1]);
                }
                else if (request[0] == "2")
                {
                    Logging.Info($"GET {request[1]}", this.endpoint);
                    this.SendFile(request[1]);
                }
                else
                {
                    this.SendError("Invalid request");
                }
            }

            Logging.Info("DISCONNECTED", this.endpoint);
            this.tcpClient.Close();
        }

        private void ListDirectory(string path)
        {
            DirectoryInfo dirInfo = new(Path.Combine(Directory.GetCurrentDirectory(), path));
            if (!dirInfo.Exists)
            {
                this.SendError("Directory not exists");
                return;
            }

            try
            {
                var list = new List<(string, bool)>();
                foreach (var dir in dirInfo.GetDirectories("*.*"))
                {
                    list.Add((dir.Name, true));
                }

                foreach (var file in dirInfo.GetFiles("*.*"))
                {
                    list.Add((file.Name, false));
                }

                this.writer.Write((long)list.Count);
                foreach (var name in list)
                {
                    this.writer.Write(name.Item1);
                    this.writer.Write(name.Item2);
                }

                this.writer.Flush();
            }
            catch (Exception e)
            {
                if (e is UnauthorizedAccessException || e is System.Security.SecurityException)
                {
                    this.SendError("Unauthorized");
                    return;
                }

                throw;
            }
        }

        private void SendFile(string path)
        {
            FileInfo file = new(Path.Combine(Directory.GetCurrentDirectory(), path));
            if (!file.Exists)
            {
                this.SendError("File not exists");
                return;
            }

            try
            {
                using FileStream fstream = new(file.FullName, FileMode.Open);
                this.writer.Write(file.Length);

                var buffer = new byte[BufferSize];
                var bytesRead = 0;
                while ((bytesRead = fstream.Read(buffer)) == BufferSize)
                {
                    this.writer.Write(buffer);
                }

                if (bytesRead > 0)
                {
                    this.writer.Write(new ReadOnlySpan<byte>(buffer, 0, bytesRead));
                }

                this.writer.Flush();
            }
            catch (UnauthorizedAccessException)
            {
                this.SendError("Unauthorized");
            }
        }

        private void SendError(string message)
        {
            this.writer.Write(ErrorCode);
            this.writer.Write(message);
            this.writer.Flush();
        }
    }
}
