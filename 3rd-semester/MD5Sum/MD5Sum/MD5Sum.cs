// <copyright file="MD5Sum.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MD5Sum;

using System.Reflection;
using System.Text;
using System.Transactions;

/// <summary>
/// MD5Sum class.
/// </summary>
public class MD5Sum
{
    /// <summary>
    /// Hashes directory name, subdirectories and files using md5.
    /// md5(dir) = md5(dirname + md5(subdir1) + ...  md5(file1) + ....)
    /// </summary>
    /// <param name="path"> Path to the directory to be hashed. </param>
    /// <returns> Byte array representing the MD5 hash of the directory. </returns>
    public static byte[]? HashDirectory(string path)
    {
        var directoryName = Path.GetDirectoryName(path);
        if (directoryName is null && path != "/")
        {
            throw new ArgumentNullException($"{path} is not a directory");
        }

        using var md5 = System.Security.Cryptography.MD5.Create();

        byte[] directoryNameBytes;
        if (directoryName is null)
        {
            directoryNameBytes = new byte[0];
        }
        else
        {
            directoryNameBytes = Encoding.UTF8.GetBytes(directoryName);
        }

        md5.TransformBlock(directoryNameBytes, 0, directoryNameBytes.Length, null, 0);

        var directories = Directory.GetDirectories(path).OrderBy(directory => directory, StringComparer.OrdinalIgnoreCase);
        var files = Directory.GetFiles(path).OrderBy(file => file, StringComparer.OrdinalIgnoreCase);
        var directoryHash = new List<Task<byte[]?>>();
        var fileHash = new List<Task<byte[]?>>();

        foreach (var dir in directories)
        {
            var task = Task.Run(() => HashDirectory(dir));
            directoryHash.Add(task);
        }

        foreach (var file in files)
        {
            var task = Task.Run(() => HashFile(file));
            directoryHash.Add(task);
        }

        Task.WaitAll(directoryHash);
        Task.WaitAll(fileHash);

        foreach (var task in directoryHash)
        {
            if (task.Result is null)
            {
                throw new ArgumentNullException("can't calculate md5 :(");
            }

            md5.TransformBlock(task.Result, 0, task.Result.Length, null, 0);
        }

        foreach (var task in fileHash)
        {
            if (task.Result is null)
            {
                throw new ArgumentNullException("can't calculate md5 :(");
            }

            md5.TransformBlock(task.Result, 0, task.Result.Length, null, 0);
        }

        md5.TransformFinalBlock([], 0, 0);

        return md5.Hash;
    }

    /// <summary>
    /// Hashes file name and file data using md5.
    /// md5(file) = md5(filename + file_data).
    /// </summary>
    /// <param name="path"> Path to the file to be hashed. </param>
    /// <returns> Byte array representing the MD5 hash of the file. </returns>
    public static byte[]? HashFile(string path)
    {
        var filename = Path.GetFileName(path);
        using var md5 = System.Security.Cryptography.MD5.Create();

        var filenameBytes = Encoding.UTF8.GetBytes(filename);
        md5.TransformBlock(filenameBytes, 0, filenameBytes.Length, null, 0);

        var buffer = new byte[4096];
        int bytes;
        using var file = File.OpenRead(path);
        while ((bytes = file.Read(buffer, 0, buffer.Length)) > 0)
        {
            md5.TransformBlock(buffer, 0, bytes, null, 0);
        }

        md5.TransformFinalBlock([], 0, 0);

        return md5.Hash;
    }
}
