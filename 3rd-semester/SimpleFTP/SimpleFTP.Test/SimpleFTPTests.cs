// <copyright file="SimpleFTPTests.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SimpleFTP.Test;

using System.Net.Sockets;
using SimpleFTPClient;
using SimpleFTPServer;

public class SimpleFTPTests
{
    private const string Host = "127.0.0.1";
    private const int Port = 2456;
    private string testDir = null!;
    private FTPServer? server;
    private CancellationTokenSource cts = null!;

    [SetUp]
    public async Task Setup()
    {
        this.testDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(this.testDir);

        File.WriteAllText(Path.Combine(this.testDir, "a.txt"), "AAA");
        Directory.CreateDirectory(Path.Combine(this.testDir, "dir"));
        File.WriteAllText(Path.Combine(this.testDir, "dir/b.txt"), "BBB");

        this.cts = new CancellationTokenSource();
        this.server = new FTPServer(Host, Port, 15);
        _ = Task.Run(() => this.server.StartAsync(this.cts.Token));
        this.server.WaitForReady();
    }

    [TearDown]
    public void Cleanup()
    {
        this.cts.Cancel();
        this.cts.Dispose();
        this.server?.Dispose();

        if (Directory.Exists(this.testDir))
        {
            Directory.Delete(this.testDir, true);
        }
    }

    [Test]
    public void List_ShouldReturnFilesAndFolders()
    {
        using var client = new FTPClient(Host, Port);
        var task = Task.Run(async () => await client.ConnectAsync());
        task.Wait();
        var (error, list) = client.List(this.testDir);

        Assert.That(error, Is.Null);
        Assert.That(list, Is.Not.Null);
        Assert.That(list.Any(x => x.Name == "a.txt" && !x.IsDirectory));
        Assert.That(list.Any(x => x.Name == "dir" && x.IsDirectory));
    }

    [Test]
    public void List_ShouldReturnError_ForMissingDirectory()
    {
        using var client = new FTPClient(Host, Port);
        var task = Task.Run(async () => await client.ConnectAsync());
        task.Wait();
        var (error, list) = client.List(Path.Combine(this.testDir, "does_not_exist"));

        Assert.That(error, Is.Not.Null);
        Assert.That(error, Does.Contain("Directory not exists"));
        Assert.That(list, Is.Null);
    }

    [Test]
    public void Get_ShouldDownloadFileCorrectly()
    {
        using var client = new FTPClient(Host, Port);
        var task = Task.Run(async () => await client.ConnectAsync());
        task.Wait();
        var saveTo = Path.Combine(this.testDir, "download.txt");
        var (error, size) = client.Get(Path.Combine(this.testDir, "a.txt"), saveTo);

        Assert.That(error, Is.Null);
        Assert.That(File.Exists(saveTo));
        Assert.That(File.ReadAllText(saveTo), Is.EqualTo("AAA"));
        Assert.That(size, Is.EqualTo(3));
    }

    [Test]
    public void Get_ShouldReturnError_WhenFileDoesNotExist()
    {
        using var client = new FTPClient(Host, Port);
        var task = Task.Run(async () => await client.ConnectAsync());
        task.Wait();
        var saveTo = Path.Combine(this.testDir, "download.txt");
        var (error, size) = client.Get("missing.txt", saveTo);

        Assert.That(error, Is.Not.Null);
        Assert.That(error, Does.Contain("File not exists"));
        Assert.That(size, Is.EqualTo(-1));
    }

    [Test]
    public void Server_ShouldSendTimeout_OnIdle()
    {
        using var tcpClient = new TcpClient();
        tcpClient.Connect(Host, Port);
        Thread.Sleep(TimeSpan.FromSeconds(20));

        var stream = tcpClient.GetStream();
        var reader = new BinaryReader(stream);
        var errorCode = reader.ReadInt64();
        Assert.That(errorCode, Is.EqualTo(-1));
        var error = reader.ReadString();
        Assert.That(error, Is.EqualTo("Timeout"));
    }
}
