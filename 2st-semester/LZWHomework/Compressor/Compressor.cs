namespace Compressor;

/// <summary>
/// Compressor class.
/// </summary>
public class Compressor
{
    private readonly int dictionaryMaxSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="Compressor"/> class.
    /// </summary>
    /// <param name="dictionaryMaxSize">Maximum size of the dictionary.</param>
    public Compressor(int dictionaryMaxSize = 4096)
    {
        if (dictionaryMaxSize < 256)
        {
            this.dictionaryMaxSize = 256;
        }

        this.dictionaryMaxSize = dictionaryMaxSize;
    }

    /// <summary>
    /// Compresses the file.
    /// </summary>
    /// <param name="inFile">The path to the file to be compressed.</param>
    /// <param name="outFile">The path to the file the compressed data to be written to.</param>
    /// <returns>true if the file was compressed successfully; otherwise false.</returns>
    public bool Compress(string inFile, string outFile)
    {
        Lzw lzwCompression = new(this.dictionaryMaxSize);
        Huffman huffmanCompression = new(this.dictionaryMaxSize);
        string tempFile = Path.GetTempFileName();

        long[]? frequencies;
        using (var input = File.OpenRead(inFile))
        using (var output = File.OpenWrite(tempFile))
        {
            frequencies = lzwCompression.Encode(input, output);
        }

        if (frequencies is null)
        {
            return false;
        }

        if (File.Exists(outFile))
        {
            return false;
        }

        using (var output = File.OpenWrite(outFile))
        using (var input = File.OpenRead(tempFile))
        {
            huffmanCompression.Encode(input, output, frequencies);
        }

        return true;
    }

    /// <summary>
    /// Decompresses the file.
    /// </summary>
    /// <param name="inFile">The path to the file to be decompressed.</param>
    /// <param name="outFile">The path to the file the decompressed data to be written to.</param>
    /// <returns>true if the file was decompressed successfully; otherwise false.</returns>
    public bool Decompress(string inFile, string outFile)
    {
        Lzw lzwCompression = new(this.dictionaryMaxSize);
        Huffman huffmanCompression = new(this.dictionaryMaxSize);

        var tempFile = Path.GetTempFileName();
        using (var input = File.OpenRead(inFile))
        using (var output = File.OpenWrite(tempFile))
        {
            huffmanCompression.Decode(input, output);
        }

        if (File.Exists(outFile))
        {
            return false;
        }

        using (var input = File.OpenRead(tempFile))
        using (var output = File.OpenWrite(outFile))
        {
            lzwCompression.Decode(input, output);
        }

        return true;
    }
}
