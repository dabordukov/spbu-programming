using MyNUnit.Attributes;

namespace TestApplication;

public class MainTests
{
    [Test]
    public void SuccessfulTest()
    {
        int x = 10;
        int y = 20;
        if (x + y != 30) throw new Exception("Math is broken");
    }

    [Test(Ignore = "Ignored test")]
    public void IgnoredTest()
    {
        throw new NotImplementedException("Should be ignored");
    }

    [Test(Expected = typeof(ArgumentException))]
    public void ExpectedExceptionTest()
    {
        throw new ArgumentException("Correct exception");
    }

    [Test(Expected = typeof(NullReferenceException))]
    public void WrongExceptionTest()
    {
        throw new InvalidOperationException("Wron Exception Test");
    }

    [Test]
    public void FailingTest()
    {
        throw new Exception("Should fail");
    }

}
