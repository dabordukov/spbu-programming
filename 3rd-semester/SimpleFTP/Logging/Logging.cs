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
    /// <summary>
    /// Logs a message to the console with a timestamp and optional tags.
    /// </summary>
    /// <param name="message"> The message to log. </param>
    /// <param name="type"> The type of the message (e.g., INFO, ERROR). </param>
    /// <param name="tags"> Optional tags to include in the log. </param>
    public static void Log(string message, string type, params string[] tags)
    {
        Console.Write($"[{type}][{DateTime.Now}]");
        foreach (var tag in tags)
        {
            Console.Write($"[{tag}]");
        }

        Console.WriteLine($"  {message}");
    }

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message"> The message to log. </param>
    /// <param name="tags"> Optional tags to include in the log. </param>
    public static void Info(string message, params string[] tags)
    {
        Log(message, "INFO", tags);
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message"> The message to log. </param>
    /// <param name="tags"> Optional tags to include in the log. </param>
    public static void Error(string message, params string[] tags)
    {
        Log(message, "ERROR", tags);
    }
}
