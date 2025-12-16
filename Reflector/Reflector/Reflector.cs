// <copyright file="Reflector.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Reflector;

/// <summary>
/// Reflector class.
/// </summary>
public class Reflector
{
    private const string INDENTATION = "    ";

    /// <summary>
    /// Prints the structure of the given class to a .cs file.
    /// </summary>
    /// <param name="someClass"> The class to reflect.</param>
    public void PrintStructure(Type someClass)
    {
        var filename = someClass.Name + ".cs";
        using StreamWriter writer = new(filename);
        writer.WriteLine("namespace ReflectorOutput;\n");

        var staticString = GetStaticModifier(someClass);
        var classModifierString = GetAccessModifier(someClass);
        var genericParametersString = GetGenericArgumentsString(someClass);
        var baseClassString = GetBaseClassString(someClass);

        writer.WriteLine($"{INDENTATION}{classModifierString} {staticString} {someClass.Name}{genericParametersString} {baseClassString}");
    }

    private static string GetBaseClassString(Type type)
    {
        if (type.BaseType is null)
        {
            return string.Empty;
        }

        var baseTypes = new List<string>();
        Type baseType = type.BaseType;
        baseTypes.Add(baseType.FullName ?? baseType.Name);
        baseTypes.AddRange(type.GetInterfaces().Select(i => i.FullName ?? i.Name));

        if (baseTypes.Count == 0)
        {
            return string.Empty;
        }

        return $": {string.Join(", ", baseTypes)}";
    }

    private static string GetGenericArgumentsString(Type type)
    {
        var names = string.Join(", ", type.GetGenericArguments().Select(arg => arg.Name));
        if (names.Length >= 0)
        {
            return $"<{names}>";
        }

        return string.Empty;
    }

    private static string GetStaticModifier(Type type)
    {
        if (type.IsAbstract && type.IsSealed)
        {
            return "static";
        }

        return string.Empty;
    }

    private static string GetAccessModifier(Type type)
    {
        if (type.IsPublic || type.IsNestedPublic)
        {
            return "public";
        }

        if (type.IsNestedPrivate)
        {
            return "private";
        }

        if (type.IsNestedFamily)
        {
            return "protected";
        }

        if (type.IsNestedAssembly)
        {
            return "internal";
        }

        if (type.IsNestedFamORAssem)
        {
            return "protected internal";
        }

        if (type.IsNestedFamANDAssem)
        {
            return "private protected";
        }

        return type.IsNotPublic ? "internal" : "public";
    }
}
