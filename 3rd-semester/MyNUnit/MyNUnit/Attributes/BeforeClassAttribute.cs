// <copyright file="BeforeClassAttribute.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyNUnit.Attributes;

/// <summary>
/// Attribute for marking methods that should be executed once before all test methods in the class.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class BeforeClassAttribute : Attribute
{
}
