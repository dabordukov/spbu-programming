// <copyright file="SparseVectorTest.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SparseVector.Test;

public class SparseVectorTest
{
    [Test]
    public void Constructor_EmptyVector_IsNull()
    {
        var vector = new SparseVector<int>(0);
        Assert.That(vector.IsNull(), Is.True);
        Assert.That(vector.Size, Is.EqualTo(0));
    }

    [Test]
    public void Constructor_FromList_CreatesCorrectVector()
    {
        var list = new List<int> { 1, 0, 3, 0, 5 };
        var vector = new SparseVector<int>(list);
        Assert.That(vector.Size, Is.EqualTo(5));
        Assert.That(vector.IsNull(), Is.False);
    }

    [Test]
    public void Addition_TwoVectors_ReturnsCorrectResult()
    {
        var v1 = new SparseVector<int>(new[] { 1, 2, 0, 4 });
        var v2 = new SparseVector<int>(new[] { 0, 3, 0, 1 });
        var result = v1 + v2;
        Assert.That(result.Size, Is.EqualTo(4));
    }

    [Test]
    public void Subtraction_TwoVectors_ReturnsCorrectResult()
    {
        var v1 = new SparseVector<double>(new[] { 1.0, 2.0, 0.0 });
        var v2 = new SparseVector<double>(new[] { 1.0, 1.0, 0.0 });
        var result = v1 - v2;
        Assert.That(result.Size, Is.EqualTo(3));
    }

    [Test]
    public void Multiplication_TwoVectors_ReturnsCorrectResult()
    {
        var v1 = new SparseVector<int>(new[] { 1, 2, 3 });
        var v2 = new SparseVector<int>(new[] { 2, 0, 4 });
        var result = v1 * v2;
        Assert.That(result.Size, Is.EqualTo(3));
    }

    [Test]
    public void Operation_DifferentSizedVectors_ThrowsException()
    {
        var v1 = new SparseVector<int>(new[] { 1, 2 });
        var v2 = new SparseVector<int>(new[] { 1, 2, 3 });

        Assert.Throws<InvalidOperationException>(() => _ = v1 + v2);
        Assert.Throws<InvalidOperationException>(() => _ = v1 - v2);
        Assert.Throws<InvalidOperationException>(() => _ = v1 * v2);
    }

    [Test]
    public void Operation_WithZeroElements_PreservesSparsity()
    {
        var v1 = new SparseVector<int>(new[] { 0, 2, 0, 4 });
        var v2 = new SparseVector<int>(new[] { 1, 0, 0, 0 });
        var result = v1 + v2;
        Assert.That(result.Size, Is.EqualTo(4));
    }

    [Test]
    public void Operation_WithDoubleType_Works()
    {
        var v1 = new SparseVector<double>(new[] { 1.5, 2.5 });
        var v2 = new SparseVector<double>(new[] { 0.5, 1.5 });
        var result = v1 + v2;
        Assert.That(result.Size, Is.EqualTo(2));
    }
}
