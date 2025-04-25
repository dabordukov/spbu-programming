// <copyright file="ExpressionTree.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace ExpressionTree;

using System.Text;

/// <summary>
/// Operations.
/// </summary>
public enum Operation
{
    /// <summary>
    /// Multiply.
    /// </summary>
    Multiply = '*',

    /// <summary>
    /// Add.
    /// </summary>
    Plus = '+',

    /// <summary>
    /// Subtract.
    /// </summary>
    Minus = '-',

    /// <summary>
    /// Divide.
    /// </summary>
    Divide = '/',
}

/// <summary>
/// Represents an expression tree.
/// </summary>
public class ExpressionTree
{
    private INode? tree;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
    /// </summary>
    /// <param name="path>">The path to the file containing the expression tree.</param>
    public ExpressionTree(string path)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// A node of the expression tree.
    /// </summary>
    private interface INode
    {
        /// <summary>
        /// Evaluates the subtree.
        /// </summary>
        /// <returns>The result of the evaluation.</returns>
        public int Evaluate();

        /// <summary>
        /// Prints the subtree.
        /// </summary>
        public void Print();
    }

    private class Operator(Operation operation, INode leftOperand, INode rightOperand) : INode
    {
        private readonly Operation operation = operation;
        private readonly INode leftOperand = leftOperand;
        private readonly INode rightOperand = rightOperand;

        public int Evaluate()
        {
            return this.operation switch
            {
                Operation.Plus => this.leftOperand.Evaluate() + this.rightOperand.Evaluate(),
                Operation.Minus => this.leftOperand.Evaluate() - this.rightOperand.Evaluate(),
                Operation.Divide => this.leftOperand.Evaluate() / this.rightOperand.Evaluate(),
                Operation.Multiply => this.leftOperand.Evaluate() * this.rightOperand.Evaluate(),
                _ => throw new ArgumentException("Invalid expression"),
            };
        }

        public void Print()
        {
            Console.Write('(');
            Console.Write(this.operation);
            Console.Write(' ');
            this.leftOperand.Print();
            Console.Write(' ');
            this.rightOperand.Print();
            Console.Write(')');
        }
    }

    private class Number(int value) : INode
    {
        private readonly int value = value;

        public int Evaluate()
        {
            return this.value;
        }

        public void Print()
        {
            Console.Write(this.value);
        }
    }

    private class Parser(string path)
    {
        private readonly FileStream stream = File.OpenRead(path);
        private int depth = 0;

        ~Parser()
        {
            this.stream?.Close();
        }

        public enum Token
        {
            TreeBegin,
            TreeEnd,
            Operator,
            Number,
            EndOfFile,
        }

        public (Token Token, int Value) NextToken()
        {
            Token token;

            this.TrimLeadingWhitespaces();
            token = this.NextTokenType();

            switch (token)
            {
                case Token.TreeBegin:
                    this.depth++;
                    return (token, 0);
                case Token.Operator:
                    return (token, this.ReadOperator());
                case Token.Number:
                    return (token, this.ReadNumber());
                case Token.TreeEnd:
                    this.depth--;
                    if (this.depth < 0)
                    {
                        throw new FormatException("Number of closing brackets is greater than number of opening ones");
                    }

                    return (token, 0);
                default:
                    return (token, 0);
            }
        }

        private int ReadOperator()
        {
            this.TrimLeadingWhitespaces();
            int nextByte = this.stream.ReadByte();
            if (!Enum.IsDefined(typeof(Operation), nextByte))
            {
                throw new FormatException($"Unknown operation: {(char)nextByte}");
            }

            return nextByte;
        }

        private int ReadNumber()
        {
            StringBuilder number = new();

            int nextByte = -1;
            while ((nextByte = this.stream.ReadByte()) >= '0' && nextByte <= '9')
            {
                number.Append(nextByte);
            }

            this.StreamBack(1);

            if (!int.TryParse(number.ToString(), out int value))
            {
                throw new OverflowException($"{number} is longer than int32");
            }

            return value;
        }

        private void StreamBack(int steps)
        {
            this.stream.Seek(this.stream.Position - steps, SeekOrigin.Begin);
        }

        private void TrimLeadingWhitespaces()
        {
            int nextByte = -1;
            while ((nextByte = this.stream.ReadByte()) == ' ' || nextByte == '\t')
            {
            }

            this.StreamBack(1);
        }

        private Token NextTokenType()
        {
            int nextByte = this.stream.ReadByte();
            if (nextByte == '(')
            {
                return Token.TreeBegin;
            }
            else if (nextByte >= '0' && nextByte <= '9')
            {
                this.StreamBack(1);
                return Token.Number;
            }
            else if (nextByte == ')')
            {
                return Token.TreeEnd;
            }
            else if (nextByte == -1)
            {
                return Token.EndOfFile;
            }
            else
            {
                throw new FormatException($"Unknown character: {(char)nextByte}");
            }
        }
    }
}
