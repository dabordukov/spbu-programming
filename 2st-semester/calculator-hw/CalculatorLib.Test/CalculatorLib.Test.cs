namespace CalculatorLib.Tests;

using Calculator;


public class CalculatorTests
{
    [TestCase("3 +5", ExpectedResult = "8")]
    [TestCase("10 - 2", ExpectedResult = "8")]
    [TestCase("4* 2", ExpectedResult = "8")]
    [TestCase(" 16/2 ", ExpectedResult = "8")]
    public string Calculate_ValidExpression_ReturnsExpectedResult(string expression)
    {
        return Calculator.Calculate(expression);
    }

    [TestCase( "3", "5", '+', ExpectedResult = "8")]
    [TestCase( "10", "2", '-', ExpectedResult = "8")]
    [TestCase( "4", "2", '*', ExpectedResult = "8")]
    [TestCase("16", "2", '/', ExpectedResult = "8")]
    public string Calculate_ValidOperandsAndOperation_ReturnsExpectedResult(string a, string b, char operation)
    {
        return Calculator.Calculate(a, b, operation);
    }

    [Test]
    public void Calculate_InvalidExpression_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Calculator.Calculate("invalid expression"));
    }

    [Test]
    public void Calculate_DivideByZero_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Calculator.Calculate("10 / 0"));
    }

    [Test]
    public void Calculate_InvalidOperands_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Calculator.Calculate("a", "b", '+'));
    }
}