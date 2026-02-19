using MyNUnit.Attributes;

namespace TestApplication;

public class AttributesTests
{
    public static List<string> order = [];

    [BeforeClass]
    public static void GlobalSetup() => order.Add("BeforeClass");

    [Before]
    public void Setup() => order.Add("Before");

    [Test]
    public void TestMethod() => order.Add("Test");

    [After]
    public void Teardown() => order.Add("After");

    [AfterClass]
    public static void GlobalTeardown() => order.Add("AfterClass");
}
