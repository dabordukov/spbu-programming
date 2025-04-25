// <copyright file="ExpressionTree.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace ExpressionTree;

using System.Data;
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
    private readonly INode? root;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
    /// </summary>
    /// <param name="path>">The path to the file containing the expression tree.</param>
    public ExpressionTree(string path)
    {
        var parser = new Parser(path);
        var stack = new Stack<Operator>();
        var leftOperand = true;

        var (token, value) = parser.NextToken();
        if (token != Parser.Token.TreeBegin)
        {
            throw new FormatException("File must contain a tree");
        }

        (token, value) = parser.NextToken();
        if (token != Parser.Token.Operator)
        {
            throw new FormatException("File must contain a tree");
        }

        this.root = new Operator((Operation)value);

        var currentNode = (Operator)this.root;
        while (token != Parser.Token.EndOfFile)
        {
            (token, value) = parser.NextToken();

            if (token == Parser.Token.TreeBegin)
            {
                (token, value) = parser.NextToken();

                if (token != Parser.Token.Operator)
                {
                    throw new FormatException("A tree should contain operation");
                }

                if (leftOperand)
                {
                    currentNode.LeftOperand = new Operator((Operation)value);
                    stack.Push(currentNode);
                    currentNode = (Operator)currentNode.LeftOperand;
                }
                else
                {
                    leftOperand = true;
                    currentNode.RightOperand = new Operator((Operation)value);
                    currentNode = (Operator)currentNode.RightOperand;
                }
            }
            else if (token == Parser.Token.Number)
            {
                if (leftOperand)
                {
                    leftOperand = false;
                    currentNode.LeftOperand = new Number(value);
                }
                else
                {
                    currentNode.RightOperand = new Number(value);
                    if (stack.Count == 0)
                    {
                        break;
                    }

                    currentNode = stack.Pop();
                }
            }
        }

        (token, value) = parser.NextToken();
        while (token != Parser.Token.EndOfFile)
        {
            if (token != Parser.Token.TreeEnd)
            {
                throw new FormatException("Invalid tree");
            }

            (token, value) = parser.NextToken();
        }
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

    /// <summary>
    /// Evaluates the expression tree.
    /// </summary>
    /// <returns>The result of the evaluation.</returns>
    public int Evaluate()
    {
        if (this.root is null)
        {
            throw new ArgumentNullException();
        }

        return this.root.Evaluate();
    }

    /// <summary>
    /// Prints the expression tree.
    /// </summary>
    public void Print()
    {
        if (this.root is null)
        {
            throw new ArgumentNullException();
        }

        this.root.Print();
    }

    private static bool IsDigit(int character)
    {
        if (character >= '0' && character <= '9')
        {
            return true;
        }

        return false;
    }

    private class Operator(Operation operation) : INode
    {
        public Operation Operation { get; } = operation;

        public INode? LeftOperand { get; set; } = null;

        public INode? RightOperand { get; set; } = null;

        public int Evaluate()
        {
            if (this.LeftOperand is null || this.RightOperand is null)
            {
                throw new FormatException();
            }

            return this.Operation switch
            {
                Operation.Plus => this.LeftOperand.Evaluate() + this.RightOperand.Evaluate(),
                Operation.Minus => this.LeftOperand.Evaluate() - this.RightOperand.Evaluate(),
                Operation.Divide => this.LeftOperand.Evaluate() / this.RightOperand.Evaluate(),
                Operation.Multiply => this.LeftOperand.Evaluate() * this.RightOperand.Evaluate(),
                _ => throw new ArgumentException("Invalid expression"),
            };
        }

        public void Print()
        {
            if (this.LeftOperand is null || this.RightOperand is null)
            {
                throw new FormatException();
            }

            Console.Write('(');
            Console.Write(this.Operation);
            Console.Write(' ');
            this.LeftOperand.Print();
            Console.Write(' ');
            this.RightOperand.Print();
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
            int minus = 1;
            int nextByte = -1;
            if ((nextByte = this.stream.ReadByte()) == '-')
            {
                minus = -1;
            }
            else
            {
                this.StreamBack(1);
            }

            while ((nextByte = this.stream.ReadByte()) >= '0' && nextByte <= '9')
            {
                number.Append((char)nextByte);
            }

            this.StreamBack(1);

            if (!int.TryParse(number.ToString(), out int value))
            {
                throw new OverflowException($"{number} is longer than int32");
            }

            return minus * value;
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

            if (nextByte != -1)
            {
                this.StreamBack(1);
            }
        }

        private Token NextTokenType()
        {
            int nextByte = this.stream.ReadByte();
            if (nextByte == '(')
            {
                return Token.TreeBegin;
            }
            else if (IsDigit(nextByte))
            {
                this.StreamBack(1);
                return Token.Number;
            }
            else if (nextByte == '-')
            {
                nextByte = this.stream.ReadByte();
                this.StreamBack(2);
                if (IsDigit(nextByte))
                {
                    return Token.Number;
                }

                return Token.Operator;
            }
            else if (nextByte is '+' or '/' or '*')
            {
                this.StreamBack(1);
                return Token.Operator;
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
