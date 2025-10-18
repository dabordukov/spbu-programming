// <copyright file="FTPClient.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SimpleFTPClient;

using System.Net.Quic;
using System.Net.Sockets;
using System.Runtime.InteropServices;

/// <summary>
/// FTPClient class.
/// </summary>
public class FTPClient : IDisposable
{
    private const int BufferSize = 4096;
    private const long ErrorCode = -1;
    private const string ListCommand = "1";
    private const string GetCommand = "2";
    private readonly TcpClient tcpClient = new();
    private readonly NetworkStream stream;
    private readonly StreamWriter writer;
    private readonly BinaryReader reader;

    /// <summary>
    /// Initializes a new instance of the <see cref="FTPClient"/> class.
    /// </summary>
    /// <param name="ip"> IP address of the FTP server. </param>
    /// <param name="port"> Port of the FTP server. </param>
    /// <param name="token"> Cancellation token. </param>
    public FTPClient(string ip, int port, CancellationToken token = default)
    {
        this.tcpClient.Connect(ip, port);
        this.stream = this.tcpClient.GetStream();
        this.reader = new BinaryReader(this.stream);
        this.writer = new StreamWriter(this.stream);
    }

    /// <summary>
    /// Disposes the FTPClient.
    /// </summary>
    public void Dispose()
    {
        this.reader.Dispose();
        this.writer.Dispose();
        this.tcpClient.Dispose();
    }

    /// <summary>
    /// Lists files on the FTP server.
    /// </summary>
    /// <param name="path"> Path to list files from. </param>
    /// <returns> A tuple containing an error message if any, and the list of files. </returns>
    public (string? Error, List<(string Name, bool IsDirectory)>? List) List(string path)
    {
        this.SendCommand(ListCommand, path);

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
        this.SendCommand(GetCommand, path);

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
        this.writer.Write(command);
        this.writer.Write(' ');
        this.writer.Write(path);
        this.writer.Write('\n');
        this.writer.Flush();
    }
}
