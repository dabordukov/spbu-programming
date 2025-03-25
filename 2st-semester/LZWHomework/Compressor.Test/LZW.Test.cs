namespace Compressor.Test;

using System.Text;

public class LzwTests
{
    [Test]
    public void EncodeDecodeTest()
    {
        string originalText = "TOBEORNOTTOBEORTOBEORNOT";
        byte[] originalBytes = Encoding.UTF8.GetBytes(originalText);

        using var inputStream = new MemoryStream(originalBytes);
        using var encodedStream = new MemoryStream();

        var lzw = new Lzw();
        long[]? frequencies = lzw.Encode(inputStream, encodedStream);
        Assert.That(frequencies, Is.Not.Null, "Frequencies should not be null after encoding.");

        encodedStream.Position = 0;
        using var decodedStream = new MemoryStream();
        lzw.Decode(encodedStream, decodedStream);

        string decodedText = Encoding.UTF8.GetString(decodedStream.ToArray());

        Assert.That(decodedText, Is.EqualTo(originalText), "The decoded text should match the original text.");
    }

    [Test]
    public void EmptyFileTest()
    {
        using var emptyInput = new MemoryStream();
        using var encodedStream = new MemoryStream();

        var lzw = new Lzw();

        long[]? frequencies = lzw.Encode(emptyInput, encodedStream);
        Assert.That(frequencies, Is.Null, "Frequencies should be null after encoding.");

        Assert.That(encodedStream.Length, Is.EqualTo(0), "Encoded stream should be empty for an empty input.");
    }
}
