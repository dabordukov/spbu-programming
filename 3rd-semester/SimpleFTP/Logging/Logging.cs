// <copyright file="Logging.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Logging;

/// <summary>
/// Logging class.
/// </summary>
public static class Logging
{
    private static readonly Lock Lock = new();

    /// <summary>
    /// Logs a message to the console with a timestamp and optional tags.
    /// </summary>
    /// <param name="stream"> The stream to write logs in. </param>
    /// <param name="message"> The message to log. </param>
    /// <param name="type"> The type of the message (e.g., INFO, ERROR). </param>
    /// <param name="tags"> Optional tags to include in the log. </param>
    public static void Log(StreamWriter stream, string message, string type, params string[] tags)
    {
        lock (Lock)
        {
            stream.Write($"[{type}][{DateTime.Now}]");
            foreach (var tag in tags)
            {
                stream.Write($"[{tag}]");
            }

            stream.WriteLine($"  {message}");
        }
    }

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="stream"> The stream to write logs in. </param>
    /// <param name="message"> The message to log. </param>
    /// <param name="tags"> Optional tags to include in the log. </param>
    public static void Info(StreamWriter stream, string message, params string[] tags)
    {
        Log(stream, message, "INFO", tags);
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="stream"> The stream to write logs in. </param>
    /// <param name="message"> The message to log. </param>
    /// <param name="tags"> Optional tags to include in the log. </param>
    public static void Error(StreamWriter stream, string message, params string[] tags)
    {
        Log(stream, message, "ERROR", tags);
    }
}
