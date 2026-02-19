// <copyright file="BeforeAttribute.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyNUnit.Attributes;

/// <summary>
/// Attribute for marking methods that should be executed before each test method in the class.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class BeforeAttribute : Attribute
{
}
