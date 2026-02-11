
namespace Compressor.Test;
public class HuffmanTests
{
    [Test]
    public void HuffmanDecodeShouldRestoreCorrectData()
    {
        int[] codes = [0, 1, 2, 0, 1];
        long[] frequencies = [2, 2, 1];
        string inputPath = Path.GetTempFileName();
        string encodedPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string decodedPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        using (var fs = File.Create(inputPath))
        {
            BinaryWriter writer = new(fs);
            foreach (var code in codes)
            {
                writer.Write(code);
            }
        }

        var huffman = new Huffman(256);
        bool encodeResult;
        using (var input = File.OpenRead(inputPath))
        using (var output = File.Create(encodedPath))
        {
            encodeResult = huffman.Encode(input, output, frequencies);
        }

        Assert.That(encodeResult, Is.True, "Encoding should return true on non-empty input.");

        bool decodeResult;
        using (var input = File.OpenRead(encodedPath))
        using (var output = File.Create(decodedPath))
        {
            decodeResult = huffman.Decode(input, output);
        }

        Assert.That(decodeResult, Is.True, "Decode should return true on non-empty input.");

        List<int> decodedCodes = [];
        using (var input = File.OpenRead(decodedPath))
        {
            BinaryReader reader = new(input);
            while (input.Position < input.Length)
            {
                decodedCodes.Add(reader.ReadInt32());
            }
        }

        Assert.That(decodedCodes, Has.Count.EqualTo(codes.Length), "Decoded data should be equal to original");
        for (var i = 0; i < decodedCodes.Count; i++)
        {
            Assert.That(decodedCodes[i], Is.EqualTo(codes[i]), "Decoded data should be equal to original");
        }

        File.Delete(inputPath);
        File.Delete(encodedPath);
        File.Delete(decodedPath);
    }

    [Test]
    public void HuffmanEncodeEmptyStreamShouldReturnFalse()
    {
        long[] frequencies = [0, 0, 0];
        string inputPath = Path.GetTempFileName();
        string encodedPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var huffman = new Huffman(256);
        bool result;
        using (var input = File.OpenRead(inputPath))
        using (var output = File.Create(encodedPath))
        {
            result = huffman.Encode(input, output, frequencies);
        }
        Assert.That(result, Is.False, "Encoding should return false for an empty input stream.");

        File.Delete(inputPath);
        File.Delete(encodedPath);
    }

    [Test]
    public void HuffmanDecodeEmptyStreamShouldReturnFalse()
    {
        string encodedPath = Path.GetTempFileName();
        string decodedPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        bool result;
        var huffman = new Huffman(256);
        using (var input = File.OpenRead(encodedPath))
        using (var output = File.Create(decodedPath))
        {
            result = huffman.Decode(input, output);
        }
        Assert.That(result, Is.False, "Decoding should return false for an empty input stream.");

        File.Delete(encodedPath);
        File.Delete(decodedPath);
    }
}
