// <copyright file="FTPServer.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SimpleFTPServer;

using System.Net;
using System.Net.Sockets;
using Logging;

/// <summary>
/// FTPServer listens for incoming TCP connections and services a simple binary/text protocol
/// that supports directory listing and file retrieval.
/// </summary>
/// <param name="ip"> IP address to listen on. </param>
/// <param name="port"> Port to listen on. </param>
/// <param name="stream"> Stream to write logs. </param>
/// <param name="clientTimeoutSeconds"> Timeout in seconds for each client. </param>
public class FTPServer(string ip, int port, Stream stream, int clientTimeoutSeconds = 60) : IDisposable
{
    private const long ErrorCode = -1;
    private readonly TcpListener tcpListener = new(IPAddress.Parse(ip), port);
    private readonly StreamWriter loggerStream = new(stream);
    private readonly List<Task> activeTasks = [];
    private readonly ManualResetEventSlim readyEvent = new(false);
    private int clientTimeoutSeconds = clientTimeoutSeconds;

    /// <summary>
    /// Initializes a new instance of the <see cref="FTPServer"/> class.
    /// Sets logger stream to console stdout.
    /// </summary>
    /// <param name="ip"> IP address to listen on. </param>
    /// <param name="port"> Port to listen on. </param>
    /// <param name="clientTimeoutSeconds"> Client timeout in seconds. </param>
    public FTPServer(string ip, int port, int clientTimeoutSeconds = 60)
    : this(ip, port, Console.OpenStandardOutput(), clientTimeoutSeconds)
        => this.loggerStream.AutoFlush = true;

    /// <summary>
    /// Disposes the FTPServer.
    /// </summary>
    public void Dispose()
        => this.tcpListener.Dispose();

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
            this.readyEvent.Set();
            Logging.Info(this.loggerStream, $"Server started on {this.tcpListener.Server.LocalEndPoint}");
            while (!token.IsCancellationRequested)
            {
                var tcpClient = await this.tcpListener.AcceptTcpClientAsync(token);
                var task = Task.Run(
                async () =>
                {
                    try
                    {
                        using var connection = new TcpConnectionProcessor(this.loggerStream, tcpClient, this.clientTimeoutSeconds);
                        await connection.ProcessConnection(token);
                    }
                    catch (Exception e)
                    {
                        Logging.Error(this.loggerStream, $"New exception: {e.Message}");
                    }
                },
                token);
                this.activeTasks.Add(task);
                this.activeTasks.RemoveAll(task => task.IsCompleted);
            }
        }
        catch (Exception e)
        {
            Logging.Error(this.loggerStream, $"New exception: {e.Message}");
            throw;
        }
        finally
        {
            this.tcpListener.Stop();
            await Task.WhenAll(this.activeTasks).WaitAsync(TimeSpan.FromSeconds(5));
            Logging.Info(this.loggerStream, "Server stopped");
        }
    }

    /// <summary>
    /// Blocks thread until server started.
    /// </summary>
    public void WaitForReady()
        => this.readyEvent.Wait();

    private class TcpConnectionProcessor : IDisposable
    {
        private const int BufferSize = 4096;
        private readonly TcpClient tcpClient;
        private readonly NetworkStream stream;
        private readonly StreamReader reader;
        private readonly BinaryWriter writer;
        private readonly string endpoint;
        private readonly StreamWriter loggerStream;
        private readonly int clientTimeoutSeconds;

        public TcpConnectionProcessor(StreamWriter stream, TcpClient tcpClient, int clientTimeoutSeconds)
        {
            this.tcpClient = tcpClient;
            this.clientTimeoutSeconds = clientTimeoutSeconds;
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

            this.loggerStream = stream;
        }

        public void Dispose()
        {
            this.reader.Dispose();
            this.writer.Dispose();
            this.stream.Dispose();
            this.tcpClient.Dispose();
        }

        public async Task ProcessConnection(CancellationToken token)
        {
            Logging.Info(this.loggerStream, "CONNECTED", this.endpoint);
            while (!token.IsCancellationRequested)
            {
                using var timeoutTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(this.clientTimeoutSeconds));
                using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, timeoutTokenSource.Token);
                string? line;
                try
                {
                    line = await this.reader.ReadLineAsync(linkedTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    Logging.Info(this.loggerStream, "TIMEOUT", this.endpoint);
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
                    Logging.Info(this.loggerStream, $"LIST {request[1]}", this.endpoint);
                    this.ListDirectory(request[1]);
                }
                else if (request[0] == "2")
                {
                    Logging.Info(this.loggerStream, $"GET {request[1]}", this.endpoint);
                    this.SendFile(request[1]);
                }
                else
                {
                    this.SendError("Invalid request");
                }
            }

            Logging.Info(this.loggerStream, "DISCONNECTED", this.endpoint);
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
