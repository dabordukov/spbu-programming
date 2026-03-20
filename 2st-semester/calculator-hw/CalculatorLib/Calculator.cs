using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Calculator;

public class Calculator
{
    /// <summary>
    /// Calculates the result of a mathematical operation between two numbers.
    /// </summary>
    /// <param name="expression">The expression to be calculated.</param>
    /// <returns>The result of the operation.</returns>
    public static string Calculate(string expression)
    {
        expression = expression.Replace(" ", "");
        var regex = new Regex(@"[+-/*]");
        
        var matches = regex.Matches(expression);
        if (matches.Count != 1)
        {
            throw new ArgumentException("Expression must be in the format 'number1 operator number2'");
        }

        var operationString = matches[0].Groups[0].Value;
        if (operationString.Length != 1)
        {
            throw new ArgumentException("Expression must be in the format 'number1 operator number2'");
        }
        char operation = operationString[0];
        

        var parts = expression.Split(operation);
        if (parts.Length != 2)
        {
            throw new ArgumentException("Expression must be in the format 'number1 operator number2'");
        }
        var leftOperand = parts[0];
        var rightOperand = parts[1];
        return Calculate(leftOperand, rightOperand, operation);
    }

    /// <summary>
    /// Calculates the result of a mathematical operation between two numbers.
    /// </summary>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <param name="operation">The operation.</param>
    /// <returns>The result of the operation.</returns>
    public static string Calculate(string a, string b, char operation)
    {
        a = a.Replace(',', '.');
        b = b.Replace(',', '.');
        if (long.TryParse(a, out long leftOperandLong) && long.TryParse(b, out long rightOperandLong))
        {
            return Calculate(leftOperandLong, rightOperandLong, operation).ToString();
        }

        if (float.TryParse(a, CultureInfo.InvariantCulture, out float leftOperandDouble))
        {
            if (float.TryParse(b, CultureInfo.InvariantCulture, out float rightOperandDouble))
            {
                return Calculate(leftOperandDouble, rightOperandDouble, operation).ToString();
            }
        }

        throw new ArgumentException("Input values are not valid numbers");
    }

    private static T Calculate<T>(T a, T b, char operation)
            where T : INumber<T>
    {
        switch (operation)
        {
            case '+':
                return a + b;
            case '-':
                return a - b;
            case '*':
                return a * b;
            case '/':
                if (b == T.Zero)
                {
                    throw new ArgumentException("Divisor can't be zero");
                }

                return a / b;
            default:
                throw new ArgumentException($"Operation {operation} is not supported");
        }
    }
}