namespace Compressor;

/// <summary>
/// Compressor class.
/// </summary>
public static class Compressor
{
    private const int DictionaryMaxSize = 4096;

    private static readonly Lzw LzwCompression = new(DictionaryMaxSize);

    private static readonly Huffman HuffmanCompression = new(DictionaryMaxSize);

    /// <summary>
    /// Compresses the file.
    /// </summary>
    /// <param name="inFile">The path to the file to be compressed.</param>
    /// <param name="outFile">The path to the file the compressed data to be written to.</param>
    /// <returns>true if the file was compressed successfully; otherwise false.</returns>
    public static bool Compress(string inFile, string outFile)
    {
        string tempFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString();

        long[]? frequencies;
        using (var input = File.OpenRead(inFile))
        using (var output = File.OpenWrite(tempFile))
        {
            frequencies = LzwCompression.Encode(input, output);
        }

        if (frequencies is null)
        {
            return false;
        }

        if (File.Exists(outFile))
        {
            File.Delete(outFile);
        }

        using (var output = File.OpenWrite(outFile))
        using (var input = File.OpenRead(tempFile))
        {
            HuffmanCompression.Encode(input, output, frequencies);
        }

        return true;
    }

    /// <summary>
    /// Decompresses the file.
    /// </summary>
    /// <param name="inFile">The path to the file to be decompressed.</param>
    /// <param name="outFile">The path to the file the decompressed data to be written to.</param>
    /// <returns>true if the file was decompressed successfully; otherwise false.</returns>
    public static bool Decompress(string inFile, string outFile)
    {
        var tempFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString();
        using (var input = File.OpenRead(inFile))
        using (var output = File.OpenWrite(tempFile))
        {
            HuffmanCompression.Decode(input, output);
        }

        if (File.Exists(outFile))
        {
            File.Delete(outFile);
        }

        using (var input = File.OpenRead(tempFile))
        using (var output = File.OpenWrite(outFile))
        {
            LzwCompression.Decode(input, output);
        }

        return true;
    }
}
