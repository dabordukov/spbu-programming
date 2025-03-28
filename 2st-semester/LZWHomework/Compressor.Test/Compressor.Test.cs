namespace Compressor.Test;

public class CompressorTests
{
    Compressor compressor;
    [SetUp]
    public void Setup()
    {
        this.compressor = new();
    }

    [Test]
    public void DecompressedDataShouldMatchOriginal()
    {
        string originalText = "This is a test string for compression.";
        string inputFile = Path.GetTempFileName();
        string compressedFile = Path.Combine(Path.GetTempPath(), System.Guid.NewGuid().ToString());
        string decompressedFile = Path.Combine(Path.GetTempPath(), System.Guid.NewGuid().ToString());
        File.WriteAllText(inputFile, originalText);

        File.Delete(compressedFile);

        bool compressed = this.compressor.Compress(inputFile, compressedFile);
        Assert.That(compressed, Is.True, "Compression should return true for a non-empty file.");

        File.Delete(decompressedFile);

        bool decompressed = this.compressor.Decompress(compressedFile, decompressedFile);
        Assert.That(decompressed, Is.True, "Decompression should return true.");

        string result = File.ReadAllText(decompressedFile);
        Assert.That(result, Is.EqualTo(originalText), "Decompressed text should match the original.");


        File.Delete(inputFile);
        File.Delete(compressedFile);
        File.Delete(decompressedFile);
    }

    [Test]
    public void CompressEmptyFileShouldReturnFalse()
    {
        string inputFile = Path.GetTempFileName();
        string compressedFile = Path.Combine(Path.GetTempPath(), System.Guid.NewGuid().ToString());

        bool compressed = this.compressor.Compress(inputFile, compressedFile);
        Assert.That(compressed, Is.False, "Compression should return false for an empty file.");

        File.Delete(inputFile);
        File.Delete(compressedFile);
    }
}
