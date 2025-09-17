// <copyright file="Matrix.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace ParallelMatrixMultiplication;

/// <summary>
/// Represents a matrix.
/// </summary>
/// <param name="rows"> Number of rows.</param>
/// <param name="columns"> Number of columns.</param>
public class Matrix
{
    private readonly int rows;
    private readonly int columns;

    private readonly int[,] array;

    private readonly Lock threadLock = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix"/> class.
    /// </summary>
    /// <param name="rows"> Number of rows.</param>
    /// <param name="columns"> Number of columns.</param>
    public Matrix(int rows, int columns)
    {
        this.rows = rows;
        this.columns = columns;
        this.array = new int[rows, columns];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix"/> class.
    /// </summary>
    /// <param name="array">The 2D array that represents a matrix.</param>
    public Matrix(int[,] array)
    : this(array.GetLength(0), array.GetLength(1))
    {
        if (array is null)
        {
            return;
        }

        for (int row = 0; row < this.rows; row++)
        {
            for (int column = 0; column < this.columns; column++)
            {
                this.array[row, column] = array[row, column];
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix"/> class.
    /// </summary>
    /// <param name="filename"> The file that contains the matrix.</param>
    /// <exception cref="InvalidDataException"> Thrown when the file is empty or has invalid matrix.</exception>
    public Matrix(string filename)
    {
        using var file = File.Open(filename, FileMode.Open);
        using var streamReader = new StreamReader(file);

        string? line;

        line = streamReader.ReadLine();
        if (line is null)
        {
            throw new InvalidDataException($"File {filename} is empty");
        }

        var numbers = line.Split(" ");
        this.columns = numbers.Length;
        this.rows = 1;
        this.array = new int[this.rows, this.columns];

        int currentColumn = 0;
        foreach (var numberString in numbers)
        {
            if (int.TryParse(numberString, out int number))
            {
                this.array[0, currentColumn++] = number;
            }
        }

        currentColumn = 0;
        int currentRow = 1;
        while ((line = streamReader.ReadLine()) != null)
        {
            numbers = line.Split(" ");
            if (numbers.Length != this.columns)
            {
                throw new InvalidDataException("All rows must have same number of elements");
            }

            if (currentRow == this.rows)
            {
                this.array = ResizeArray(this.array, this.rows * 2, this.columns);
                this.rows *= 2;
            }

            foreach (var numberString in numbers)
            {
                if (int.TryParse(numberString, out int number))
                {
                    this.array[currentRow, currentColumn++] = number;
                }
            }

            currentRow++;
            currentColumn = 0;
        }

        this.rows = currentRow;
        this.array = ResizeArray(this.array, this.rows, this.columns);
    }

    public static bool operator ==(Matrix left, Matrix right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Matrix a, Matrix b)
    {
        return !(a == b);
    }

    /// <summary>
    /// Multiplies two matrices in a single thread.
    /// </summary>
    /// <param name="left">Left matrix.</param>
    /// <param name="right">Right matrix.</param>
    /// <returns>Multiplication of matrices.</returns>
    /// <exception cref="ArgumentException">Thrown when matrices are not compatible.</exception>
    public static Matrix MultiplySingleThread(Matrix left, Matrix right)
    {
        if (!AreCompatible(left, right))
        {
            throw new ArgumentException("Matrices are not compatible");
        }

        var c = new Matrix(left.rows, right.columns);

        for (int row = 0; row < c.rows; row++)
        {
            for (int column = 0; column < c.columns; column++)
            {
                for (int k = 0; k < right.rows; k++)
                {
                    c.array[row, column] += left.array[row, k] * right.array[k, column];
                }
            }
        }

        return c;
    }

    /// <summary>
    /// Multiplies two matrices in multiple threads.
    /// </summary>
    /// <param name="left">Left matrix.</param>
    /// <param name="right">Right matrix.</param>
    /// <returns> Multiplication of matrices.</returns>
    /// <exception cref="ArgumentException">Thrown when matrices are not compatible.</exception>
    public static Matrix MultiplyMultiThread(Matrix left, Matrix right)
    {
        if (!AreCompatible(left, right))
        {
            throw new ArgumentException("Matrices are not compatible");
        }

        var c = new Matrix(left.rows, right.columns);

        var threads = new Thread[Environment.ProcessorCount];

        for (int i = 0; i < Environment.ProcessorCount && i < c.rows * c.columns; i++)
        {
            int threadIdx = i;
            threads[i] = new Thread(() => CalculateElementMultiThread(left, right, c, threadIdx));
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        return c;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns> A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// Compares two matrices for equality.
    /// </summary>
    /// <param name="other"> The other matrix.</param>
    /// <returns> True if matrices are equal, false otherwise.</returns>
    public override bool Equals(object? other)
    {
        if (other is not Matrix b)
        {
            return false;
        }

        if (this.rows != b.rows || this.columns != b.columns)
        {
            return false;
        }

        for (int row = 0; row < this.rows; row++)
        {
            for (int column = 0; column < this.columns; column++)
            {
                if (this.array[row, column] != b.array[row, column])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool AreCompatible(Matrix left, Matrix right)
    => right.rows == left.columns;

    private static void CalculateElementMultiThread(Matrix left, Matrix right, Matrix matrix, int threadIdx)
    {
        int row = 0;
        int column = threadIdx;
        if (column >= matrix.columns)
        {
            row += column / matrix.columns;
            column %= matrix.columns;
        }

        while (row < matrix.rows)
        {
            for (int k = 0; k < right.rows; k++)
            {
                matrix.array[row, column] += left.array[row, k] * right.array[k, column];
            }

            column += Environment.ProcessorCount;
            if (column >= matrix.columns)
            {
                row += column / matrix.columns;
                column %= matrix.columns;
            }
        }
    }

    private static T[,] ResizeArray<T>(T[,] original, int rows, int cols)
    {
        var newArray = new T[rows, cols];
        int minRows = Math.Min(rows, original.GetLength(0));
        int minCols = Math.Min(cols, original.GetLength(1));
        for (int i = 0; i < minRows; i++)
        {
            for (int j = 0; j < minCols; j++)
            {
                newArray[i, j] = original[i, j];
            }
        }

        return newArray;
    }
}
