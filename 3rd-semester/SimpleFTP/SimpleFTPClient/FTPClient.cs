// <copyright file="FTPClient.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SimpleFTPClient;

using System.Net.Sockets;

/// <summary>
/// FTPClient connects to FTP server. Supports directory listing and file retrieval.
/// </summary>
/// <param name="ip"> IP address of the FTP server. </param>
/// <param name="port"> Port of the FTP server. </param>
public class FTPClient(string ip, int port) : IDisposable
{
    private const int BufferSize = 4096;
    private const long ErrorCode = -1;
    private const string ListCommand = "1";
    private const string GetCommand = "2";
    private readonly TcpClient tcpClient = new();
    private readonly string ip = ip;
    private readonly int port = port;
    private NetworkStream? stream;
    private StreamWriter? writer;
    private BinaryReader? reader;

    /// <summary>
    /// Connects to FTP server.
    /// </summary>
    /// <param name="token"> Cancellation token. </param>
    /// <returns> A <see cref="Task"/> representing the asynchronous operation of connecting to the FTP server. </returns>
    public async Task ConnectAsync(CancellationToken token = default)
    {
        await this.tcpClient.ConnectAsync(this.ip, this.port, token);
        this.stream = this.tcpClient.GetStream();
        this.reader = new BinaryReader(this.stream);
        this.writer = new StreamWriter(this.stream);
    }

    /// <summary>
    /// Disposes the FTPClient.
    /// </summary>
    public void Dispose()
    {
        this.reader?.Dispose();
        this.writer?.Dispose();
        this.tcpClient.Dispose();
    }

    /// <summary>
    /// Lists files on the FTP server.
    /// </summary>
    /// <param name="path"> Path to list files from. </param>
    /// <returns> A tuple containing an error message if any, and the list of files. </returns>
    public (string? Error, List<(string Name, bool IsDirectory)>? List) List(string path)
    {
        if (this.reader is null)
        {
            throw new ArgumentNullException("Connection is not opened");
        }

        try
        {
            this.SendCommand(ListCommand, path);
        }
        catch (IOException)
        {
            this.tcpClient.Close();
            return ("Closed", null);
        }

        long count = this.reader.ReadInt64();
        if (count == ErrorCode)
        {
            string errorMessage = this.reader.ReadString();
            return (errorMessage, null);
        }

        var list = new List<(string, bool)>();

        for (long i = 0; i < count; i++)
        {
            string name = this.reader.ReadString();
            bool isDirectory = this.reader.ReadBoolean();

            list.Add((name, isDirectory));
        }

        return (null, list);
    }

    /// <summary>
    /// Gets a file from the FTP server.
    /// </summary>
    /// <param name="path"> Path to the file. </param>
    /// <param name="filenameSaveTo"> Local filename to save the file to. </param>
    /// <returns> A tuple containing an error message if any, and the file size. </returns>
    public (string? Error, long Size) Get(string path, string filenameSaveTo)
    {
        if (this.reader is null || this.stream is null)
        {
            throw new ArgumentNullException("Connection is not opened");
        }

        try
        {
            this.SendCommand(GetCommand, path);
        }
        catch (IOException)
        {
            this.tcpClient.Close();
            return ("Closed", -1);
        }

        var size = this.reader.ReadInt64();
        if (size == ErrorCode)
        {
            var errorMessage = this.reader.ReadString();
            return (errorMessage, -1);
        }

        using FileStream file = new(filenameSaveTo, FileMode.CreateNew);
        var buffer = new byte[BufferSize];

        long count = 0;
        var bytesRead = 0;
        while ((bytesRead = this.stream.Read(buffer)) == BufferSize && count < size)
        {
            file.Write(buffer);
            count += size;
        }

        if (bytesRead > 0 && count < size)
        {
            file.Write(new ReadOnlySpan<byte>(buffer, 0, bytesRead));
            count += bytesRead;
        }

        if (size != count)
        {
            return ("Size of file and number of received bytes are not equal :(", -1);
        }

        return (null, size);
    }

    private void SendCommand(string command, string path)
    {
        if (this.writer is null)
        {
            throw new ArgumentNullException("Connection is not opened");
        }

        this.writer.Write(command);
        this.writer.Write(' ');
        this.writer.Write(path);
        this.writer.Write('\n');
        this.writer.Flush();
    }
}
