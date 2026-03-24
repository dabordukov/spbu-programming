// <copyright file="Runner.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyNUnit;

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using MyNUnit.Attributes;

/// <summary>
/// Class for running tests and collecting results.
/// </summary>
public class Runner
{
    /// <summary>
    /// Gets the results of all test executions.
    /// </summary>
    public ConcurrentBag<Result> Results { get; } = [];

    /// <summary>
    /// Runs tests from all assemblies in the specified directory asynchronously.
    /// </summary>
    /// <param name="path"> Path to the directory containing test assemblies.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RunTestsAsync(string path)
    {
        if (!Directory.Exists(path))
        {
            return;
        }

        var files = Directory.GetFiles(path, "*.dll");

        var tasks = files.Select(file => Task.Run(() =>
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                if (!assembly.GetReferencedAssemblies().Any(reference => reference.Name == "MyNUnit"))
                {
                    return;
                }

                var testClasses = assembly.GetTypes().Where(
                    type => type.IsClass && type.GetMethods().Any(method => method.GetCustomAttribute<TestAttribute>() != null));

                var classTasks = testClasses.Select(testClass => Task.Run(() => this.RunTestsInClass(testClass))).ToArray();
                Task.WaitAll(classTasks);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Could not load file {file}: {ex.Message}");
            }
        }));

        await Task.WhenAll(tasks);
    }

    private void RunTestsInClass(Type type)
    {
        var methods = type.GetMethods();
        var testMethods = methods.Where(method => method.GetCustomAttribute<TestAttribute>() != null).ToList();

        this.InvokeMethodsByAttribute<BeforeClassAttribute>(type, null, true);

        foreach (var method in testMethods)
        {
            var attr = method.GetCustomAttribute<TestAttribute>()!;

            if (!string.IsNullOrEmpty(attr.Ignore))
            {
                this.Results.Add(new Result(type.Name, method.Name, "IGNORED", 0, attr.Ignore));
                continue;
            }

            object? testClass;
            try
            {
                testClass = Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                this.Results.Add(new Result(type.Name, method.Name, "FAILED", 0, $"Test class instance creation failed: {ex.Message}"));
                continue;
            }

            this.InvokeMethodsByAttribute<BeforeAttribute>(type, testClass, false);

            var watch = Stopwatch.StartNew();
            try
            {
                method.Invoke(testClass, null);
                watch.Stop();

                if (attr.Expected != null)
                {
                    this.Results.Add(
                        new Result(
                            type.Name,
                            method.Name,
                            "FAILED",
                            watch.ElapsedMilliseconds,
                            $"Expected {attr.Expected.Name} but method completed successfully."));
                }
                else
                {
                    this.Results.Add(new Result(type.Name, method.Name, "PASSED", watch.ElapsedMilliseconds));
                }
            }
            catch (TargetInvocationException ex)
            {
                watch.Stop();
                var actualEx = ex.InnerException?.GetType();

                if (attr.Expected != null && actualEx == attr.Expected)
                {
                    this.Results.Add(new Result(type.Name, method.Name, "PASSED", watch.ElapsedMilliseconds));
                }
                else
                {
                    this.Results.Add(new Result(type.Name, method.Name, "FAILED", watch.ElapsedMilliseconds, ex.InnerException?.Message));
                }
            }
            finally
            {
                this.InvokeMethodsByAttribute<AfterAttribute>(type, testClass, isStatic: false);
            }
        }

        this.InvokeMethodsByAttribute<AfterClassAttribute>(type, null, isStatic: true);
    }

    private void InvokeMethodsByAttribute<T>(Type type, object? instance, bool isStatic)
    where T : Attribute
    {
        var methods = type.GetMethods()
            .Where(method => method.GetCustomAttribute<T>() != null && method.IsStatic == isStatic);

        foreach (var method in methods)
        {
            try
            {
                method.Invoke(instance, null);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {typeof(T).Name} ({type.Name}): {ex.InnerException?.Message}");
            }
        }
    }

    /// <summary>
    /// Result of a single test execution, used for reporting.
    /// </summary>
    /// <param name="ClassName"> Name of the test class.</param>
    /// <param name="MethodName"> Name of the test method.</param>
    /// <param name="Status"> Test result status: PASSED, FAILED, or IGNORED.</param>
    /// <param name="DurationMs"> Execution time in milliseconds.</param>
    /// <param name="Message"> Optional message, e.g., failure reason or ignore reason.</param>
    public record Result(
        string ClassName,
        string MethodName,
        string Status,
        long DurationMs,
        string? Message = null);
}
