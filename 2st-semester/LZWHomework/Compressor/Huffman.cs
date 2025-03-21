namespace Compressor;

/// <summary>
/// Huffman algorithm implementation.
/// </summary>
public class Huffman(int dictionaryMaxSize)
{
    private readonly int dictionaryMaxSize = dictionaryMaxSize;
    private readonly List<bool>[] codeTable = new List<bool>[dictionaryMaxSize];

    /// <summary>
    /// Encodes the input using the Huffman algorithm.
    /// </summary>
    /// <param name="input">The LZW-output.</param>
    /// <param name="output">The output to write the encoded data.</param>
    /// <param name="frequencies">The frequencies of each code in the input.</param>
    /// <returns>The encoded input.</returns>
    public bool Encode(FileStream input, FileStream output, long[] frequencies)
    {
        BinaryWriter writer = new(output);
        writer.Write(frequencies.Length);
        foreach (var frequency in frequencies)
        {
            writer.Write(frequency);
        }

        var huffmanTree = BuildHuffmanTree(frequencies);

        this.BuildCodeTable(huffmanTree);

        BinaryReader reader = new(input);
        if (reader.BaseStream.Position >= reader.BaseStream.Length)
        {
            return false;
        }

        var buffer = new bool[8];
        var bufferLength = 0;
        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            var code = reader.ReadInt32();
            foreach (var b in this.codeTable[code])
            {
                buffer[bufferLength++] = b;
                if (bufferLength == 8)
                {
                    writer.Write(BoolToByte(buffer));
                    bufferLength = 0;
                }
            }
        }

        if (bufferLength != 0)
        {
            writer.Write(BoolToByte(buffer));
        }

        return true;
    }

    /// <summary>
    /// Decodes the input encoded with the Huffman algorithm.
    /// </summary>
    /// <param name="input">The input to decode.</param>
    /// <param name="output">The output to write the decoded data.</param>
    /// <returns>True if decoded successfully; otherwise false.</returns>
    public bool Decode(FileStream input, FileStream output)
    {
        BinaryReader reader = new(input);
        if (reader.BaseStream.Position >= reader.BaseStream.Length)
        {
            return false;
        }

        BinaryWriter writer = new(output);
        if (reader.BaseStream.Position >= reader.BaseStream.Length)
        {
            return false;
        }

        var frequenciesLength = reader.ReadInt32();
        var frequencies = new long[frequenciesLength];
        long decodedLength = 0;
        for (int i = 0; i < frequenciesLength; i++)
        {
            if (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                frequencies[i] = reader.ReadInt64();
                decodedLength += frequencies[i];
            }
            else
            {
                return false;
            }
        }

        var root = BuildHuffmanTree(frequencies);

        byte buffer;
        int bufferIndex;
        var currentNode = root;

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            bufferIndex = 7;
            buffer = reader.ReadByte();
            while (bufferIndex >= 0 && decodedLength > 0)
            {
                if (currentNode is null)
                {
                    return false;
                }

                if (currentNode.Code != -1)
                {
                    writer.Write(currentNode.Code);
                    decodedLength--;
                    currentNode = root;
                }

                currentNode = (buffer & (1 << bufferIndex)) == 0 ? currentNode.Left : currentNode.Right;

                bufferIndex--;
            }
        }

        return true;
    }

    private static byte BoolToByte(bool[] buffer)
    {
        byte @byte = 0;
        for (var i = 0; i < 8; i++)
        {
            if (buffer[i])
            {
                @byte |= (byte)(1 << (7 - i));
            }
        }

        return @byte;
    }

    private static Node BuildHuffmanTree(long[] frequencies)
    {
        PriorityQueue<Node, long> queue = new(Comparer<long>.Create((a, b) => (a < b) ? -1 : ((a > b) ? 1 : 0)));
        for (var i = 0; i < frequencies.Length; i++)
        {
            if (frequencies[i] != 0)
            {
                queue.Enqueue(new Node(i, frequencies[i]), frequencies[i]);
            }
        }

        while (queue.Count > 1)
        {
            var node1 = queue.Dequeue();
            var node2 = queue.Dequeue();
            var node3 = new Node(node1, node2);
            queue.Enqueue(node3, node3.Frequency);
        }

        // if the tree has only one element, it should not be the root
        var last = queue.Peek();
        if (last.Code != -1)
        {
            queue.DequeueEnqueue(new Node(last, null), last.Frequency);
        }

        return queue.Dequeue();
    }

    private void BuildCodeTableInternals(Node node, List<bool> code, int codeLength)
    {
        if (node.Code != -1)
        {
            this.codeTable[node.Code] = code[..codeLength];
            return;
        }

        code.Add(false);
        if (node.Left is not null)
        {
            this.BuildCodeTableInternals(node.Left, code, codeLength + 1);
        }

        code[codeLength] = true;
        if (node.Right is not null)
        {
            this.BuildCodeTableInternals(node.Right, code, codeLength + 1);
        }

        code.RemoveAt(code.Count - 1);
        return;
    }

    private void BuildCodeTable(Node tree)
    {
        List<bool> code = [];
        var codeLength = 0;
        this.BuildCodeTableInternals(tree, code, codeLength);
    }

    private sealed class Node
    {
        public Node(int code, long frequency)
        {
            this.Code = code;
            this.Frequency = frequency;
        }

        public Node(Node? left, Node? right)
        {
            this.Left = left;
            this.Right = right;
            this.Frequency = (left?.Frequency ?? 0) + (right?.Frequency ?? 0);
        }

        public long Frequency { get; set; } = 0;

        public int Code { get; set; } = -1;

        public Node? Left { get; set; }

        public Node? Right { get; set; }
    }
}
