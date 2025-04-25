// <copyright file="ExpressionTreeTest.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace ExpressionTree.Test;

public class ExpressionTreeTest
{
    private static readonly (string Expression, int Expected)[] CorrectExpressions =
    [("(+ (* (+ 44 1) 5) (* (+ 1 1) 2))", 229),
     ("(/ (* (+ 44 1) 5) (+ (+ 2 1) 2))", 45),
     ("(/ (* (+ 44 1) 5) (- (+ 2 1) 8))", -45),
     ("(/ (* (+ -44 1) 5) (- (+ 2 1) 8))", 43)];

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

    [Test]
    public void CorrectExpressionShouldBeEvaluatedCorrectly([ValueSource(nameof(CorrectExpressions))] (string Expression, int Expected) correctExpression)
    {
        var filename = Path.Combine(this.tempDirectory, "correct.txt");
        File.WriteAllText(filename, correctExpression.Expression);

        var tree = new ExpressionTree(filename);
        var result = tree.Evaluate();
        Assert.That(result, Is.EqualTo(correctExpression.Expected));
    }
}
