namespace Routers.Tests;

public class NetworkTests
{
    private string tempDirectory;

    [OneTimeSetUp]
    public void SetUp()
    {
        this.tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(this.tempDirectory);
    }

    [Test]
    public void MakeOptimal_ConnectedGraph_ReturnsTrue()
    {
        var filename = Path.Combine(this.tempDirectory, "connected.txt");
        File.WriteAllText(filename, "0: 1(5), 2(2)\n1: 0(5), 2(1)\n2: 0(2), 1(1)");

        var network = new Network(filename);
        var result = network.MakeOptimal();

        Assert.That(result, Is.True);
    }

    [Test]
    public void MakeOptimal_DisconnectedGraph_ReturnsFalse()
    {
        var filename = Path.Combine(this.tempDirectory, "disconnected.txt");
        File.WriteAllText(filename, "0: 1(5)\n1: 0(5)\n2: 3(1)\n3: 2(1)");

        var network = new Network(filename);
        var result = network.MakeOptimal();

        Assert.That(result, Is.False);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        if (Directory.Exists(this.tempDirectory))
        {
            Directory.Delete(this.tempDirectory, true);
        }
    }
}
