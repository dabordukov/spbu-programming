// <copyright file="ExpressionTreeTest.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace ExpressionTree.Test;

public class ExpressionTreeTest
{
    private string tempDirectory;

    [OneTimeSetUp]
    public void SetUp()
    {
        this.tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(this.tempDirectory);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        if (Directory.Exists(this.tempDirectory))
        {
            Directory.Delete(this.tempDirectory, true);
        }
    }

    [TestCase("(+ (* (+ 44 1) 5) (* (+ 1 1) 2))", ExpectedResult = 229)]
    [TestCase("(/ (* (+ 44 1) 5) (+ (+ 2 1) 2))", ExpectedResult = 45)]
    [TestCase("(/ (* (+ 44 1) 5) (- (+ 2 1) 8))", ExpectedResult = -45)]
    [TestCase("(/ (* (+ -44 1) 5) (- (+ 2 1) 8))", ExpectedResult = 43)]
    [TestCase("(/ (* (- 44 1) 5) (- (+ 2 1) 8))", ExpectedResult = -43)]
    public int CorrectExpressionShouldBeEvaluatedCorrectly(string expression)
    {
        var filename = Path.Combine(this.tempDirectory, "correct.txt");
        File.WriteAllText(filename, expression);

        var tree = new ExpressionTree(filename);
        return tree.Evaluate();
    }

    [TestCase("(/ (* (+ -44 1) 5) (- (+ 2 ) 8))")]
    [TestCase("(/ (* (+ -44 1) 5) (- (+ 2 1) ))")]
    [TestCase("(/ (* (-44 1) 5) (- (+ 2 1) 8))")]
    [TestCase("()")]
    [TestCase("(* 1 2")]
    [TestCase("* 1 2)")]
    public void ExpressionWith_EmptyToken_ShouldThrowFormatException(string expression)
    {
        var filename = Path.Combine(this.tempDirectory, "correct.txt");
        File.WriteAllText(filename, expression);

        Assert.Throws<FormatException>(() => new ExpressionTree(filename));
    }

    [TestCase("(+ 9999999999999 1)")]
    public void ExpressionWith_NumberNotFittinIn_Int32_ShouldThrowOverflowException(string expression)
    {
        var filename = Path.Combine(this.tempDirectory, "correct.txt");
        File.WriteAllText(filename, expression);

        Assert.Throws<OverflowException>(() => new ExpressionTree(filename));
    }

    [TestCase("(* 1073741824 8)")]
    public void ExpressionWith_ArithemticOverflow_ShouldThrowOverflowException(string expression)
    {
        var filename = Path.Combine(this.tempDirectory, "correct.txt");
        File.WriteAllText(filename, expression);
        var tree = new ExpressionTree(filename);
        Assert.Throws<OverflowException>(() => tree.Evaluate());
    }
}
