// <copyright file="MatrixTest.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace ParallelMatrixMultiplication.Test;

using ParallelMatrixMultiplication;

public class MatrixTest
{
    private static readonly List<(string, string, string)> Matrices = [
        ("matrix1_a", "matrix1_b", "matrix1_result"),
        ("matrix2_a", "matrix2_b", "matrix2_result"),
    ];

    private readonly string testsDirectory = "TestCases";

    [TestCaseSource(nameof(Matrices))]
    public void Multiplication_MultiThread_SquareMatrix_IsCorrect((string Left, string Right, string Result) matrices)
    {
        var matrixLeft = new Matrix(Path.Combine(this.testsDirectory, matrices.Left));
        var matrixRight = new Matrix(Path.Combine(this.testsDirectory, matrices.Right));
        var matrixResult = new Matrix(Path.Combine(this.testsDirectory, matrices.Result));
        Assert.That(Matrix.MultiplyMultiThread(matrixLeft, matrixRight), Is.EqualTo(matrixResult));
    }

    [TestCaseSource(nameof(Matrices))]
    public void Multiplication_SingleThread_SquareMatrix_IsCorrect((string Left, string Right, string Result) matrices)
    {
        var matrixLeft = new Matrix(Path.Combine(this.testsDirectory, matrices.Left));
        var matrixRight = new Matrix(Path.Combine(this.testsDirectory, matrices.Right));
        var matrixResult = new Matrix(Path.Combine(this.testsDirectory, matrices.Result));
        Assert.That(Matrix.MultiplySingleThread(matrixLeft, matrixRight), Is.EqualTo(matrixResult));
    }

    [Test]
    public void Multiplication_UncompatibleMatrices_ThrowsArgumentException()
    {
        var matrixLeft = new Matrix(Path.Combine(this.testsDirectory, "matrixNotCompatible_a"));
        var matrixRight = new Matrix(Path.Combine(this.testsDirectory, "matrixNotCompatible_b"));
        Assert.Throws<ArgumentException>(() => Matrix.MultiplySingleThread(matrixLeft, matrixRight));
        Assert.Throws<ArgumentException>(() => Matrix.MultiplyMultiThread(matrixLeft, matrixRight));
    }
}
