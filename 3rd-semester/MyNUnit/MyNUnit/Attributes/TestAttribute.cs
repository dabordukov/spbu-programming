// <copyright file="TestAttribute.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyNUnit.Attributes;

/// <summary>
/// Attribute for marking test methods.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the expected exception type. If not null, the test will pass only if the specified exception is thrown.
    /// If null, the test will pass only if no exceptions are thrown.
    /// </summary>
    public Type? Expected { get; set; }

    /// <summary>
    /// Gets or sets the ignore reason. If not null, the test will be ignored with the specified reason.
    /// </summary>
    public string? Ignore { get; set; }
}
