// <copyright file="MyNUnitTest.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyNUnit.Test;

using MyNUnit;
using NUnit.Framework;

[TestFixture]
public class MyNUnitTest
{
    [Test]
    public async Task RunnerShouldReportCorrectStatus()
    {
        var runner = new Runner();
        await runner.RunTestsAsync(TestContext.CurrentContext.TestDirectory);

        Assert.Multiple(() =>
        {
            var success = runner.Results.FirstOrDefault(r => r.MethodName == "SuccessfulTest");
            Assert.That(success?.Status, Is.EqualTo("PASSED"));

            var ignored = runner.Results.FirstOrDefault(r => r.MethodName == "IgnoredTest");
            Assert.That(ignored?.Status, Is.EqualTo("IGNORED"));

            var wrongEx = runner.Results.FirstOrDefault(r => r.MethodName == "WrongExceptionTest");
            Assert.That(wrongEx?.Status, Is.EqualTo("FAILED"));

            var expectedEx = runner.Results.FirstOrDefault(r => r.MethodName == "ExpectedExceptionTest");
            Assert.That(expectedEx?.Status, Is.EqualTo("PASSED"));
        });
    }

    [Test]
    public async Task RunnerShouldProcessAttributes()
    {
        var runner = new Runner();
        await runner.RunTestsAsync(TestContext.CurrentContext.TestDirectory);

        var testType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == "AttributesTests");

        var log = testType?.GetField("order")?.GetValue(null) as List<string>;

        var expectedOrder = new List<string>
        {
            "BeforeClass",
            "Before",
            "Test",
            "After",
            "AfterClass",
        };

        Assert.That(log, Is.EqualTo(expectedOrder));
    }
}
