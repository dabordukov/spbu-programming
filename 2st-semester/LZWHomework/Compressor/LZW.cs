namespace Compressor;

/// <summary>
/// LZW algorithm implementation.
/// </summary>
public class Lzw(int dictionaryMaxSize)
{
    private readonly int dictionaryMaxSize = dictionaryMaxSize;

    /// <summary>
    /// Encodes the file using the LZW algorithm.
    /// </summary>
    /// <param name="input">The file to be encoded.</param>
    /// <param name="output">File to write encoded data.</param>
    /// <returns>Frequencies array.</returns>
    public long[]? Encode(FileStream input, FileStream output)
    {
        BinaryWriter writer = new(output);
        Trie dictionary = new();
        byte[] currentByteArray = new byte[1];
        ReadOnlySpan<byte> currentByte = new(currentByteArray);
        for (int i = 0; i < 256; i++)
        {
            currentByteArray[0] = (byte)i;
            dictionary.Add(currentByte, i);
        }

        long[] frequencies = new long[this.dictionaryMaxSize];
        int capacity = 256;
        byte[] buffer = new byte[capacity];
        int bufferLength = 0;

        int value = 0;
        int nextByte = -1;
        while ((nextByte = input.ReadByte()) != -1)
        {
            if (bufferLength == capacity)
            {
                capacity *= 2;
                Array.Resize(ref buffer, capacity);
            }

            buffer[bufferLength++] = (byte)nextByte;

            if (dictionary.Contains(new ReadOnlySpan<byte>(buffer, 0, bufferLength)))
            {
                continue;
            }

            if (dictionary.Size < this.dictionaryMaxSize)
            {
                dictionary.Add(new ReadOnlySpan<byte>(buffer, 0, bufferLength), dictionary.Size);
            }

            if (!dictionary.TryGetValue(new ReadOnlySpan<byte>(buffer, 0, bufferLength - 1), out value))
            {
                return null;
            }

            writer.Write(value);
            frequencies[value]++;

            buffer[0] = (byte)nextByte;
            bufferLength = 1;
        }

        if (!dictionary.TryGetValue(new ReadOnlySpan<byte>(buffer, 0, bufferLength), out value))
        {
            return null;
        }

        writer.Write(value);
        frequencies[value]++;

        return frequencies;
    }

    /// <summary>
    /// Decodes the encoded sequence.
    /// </summary>
    /// <param name="input">Encoded sequence.</param>
    /// <param name="output">File to write decoded data.</param>
    public void Decode(FileStream input, FileStream output)
    {
        BinaryReader reader = new(input);
        if (reader.BaseStream.Position >= reader.BaseStream.Length)
        {
            return;
        }

        List<byte>[] codes = new List<byte>[this.dictionaryMaxSize];
        for (int i = 0; i < 256; i++)
        {
            codes[i] = [(byte)i];
        }

        int codesLength = 256;

        int previousCode = reader.ReadInt32();
        output.Write(codes[previousCode].ToArray());
        while (reader.BaseStream.Position != reader.BaseStream.Length)
        {
            var code = -1;
            code = reader.ReadInt32();

            List<byte> sequence;
            if (code < codesLength)
            {
                sequence = [.. codes[code]];
            }
            else
            {
                sequence = [.. codes[previousCode]];
                sequence.Add(codes[previousCode][0]);
            }

            output.Write(sequence.ToArray());

            if (codesLength < this.dictionaryMaxSize)
            {
                codes[codesLength++] = [.. codes[previousCode], .. sequence[..1]];
            }

            previousCode = code;
        }
    }
}
