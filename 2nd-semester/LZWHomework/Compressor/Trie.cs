namespace Compressor;

/// <summary>
/// Trie data structure.
/// </summary>
public class Trie
{
    private readonly Node root = new(0);

    /// <summary>
    /// Gets the number of elements in the Trie.
    /// </summary>
    public int Size => this.NumberOfSequences;

    private int NumberOfSequences { get; set; } = 0;

    /// <summary>
    /// Adds a sequence to the Trie.
    /// </summary>
    /// <param name="sequence">The sequence to be added to the Trie.</param>
    /// <param name="sequenceValue">The value the sequence has.</param>
    /// <returns>
    /// True if the sequence was successfully added to the Trie. <br/> False if the sequence already exists in the Trie.
    /// </returns>
    public bool Add(ReadOnlySpan<byte> sequence, int sequenceValue)
    {
        if (sequence.IsEmpty)
        {
            return false;
        }

        Node? current = this.root;
        foreach (var currentByte in sequence)
        {
            if (current is null)
            {
                return false;
            }

            if (current.Children[currentByte] is null)
            {
                current.Children[currentByte] = new Node(sequenceValue);
            }

            current = current.Children[currentByte];
        }

        if (current is null || current.IsEndOfSequence)
        {
            return false;
        }

        current.IsEndOfSequence = true;
        this.NumberOfSequences++;
        return true;
    }

    /// <summary>
    /// Checks if the trie contains the specified sequence.
    /// </summary>
    /// <param name="sequence">The sequence to search for in the trie.</param>
    /// <returns>True if the sequence is found, otherwise false.</returns>
    public bool Contains(ReadOnlySpan<byte> sequence)
    {
        if (sequence.IsEmpty)
        {
            return false;
        }

        Node? current = this.root;
        foreach (var currentByte in sequence)
        {
            if (current is null || current.Children[currentByte] is null)
            {
                return false;
            }

            current = current.Children[currentByte];
        }

        return (current is not null) && current.IsEndOfSequence;
    }

    /// <summary>
    /// Gets the value of the specified sequence.
    /// </summary>
    /// <param name="sequence">The sequence to get the value of. </param>
    /// <returns>If the sequence is found, the value of the sequence is returned. Otherwise null is returned.</returns>
    public int? GetValue(ReadOnlySpan<byte> sequence)
    {
        if (sequence.IsEmpty)
        {
            return null;
        }

        Node? current = this.root;
        foreach (var currentByte in sequence)
        {
            if (current is null || current.Children[currentByte] is null)
            {
                return null;
            }

            current = current.Children[currentByte];
        }

        if (current is null)
        {
            return null;
        }

        return current.Value;
    }

    /// <summary>
    /// Gets the value of the specified sequence.
    /// </summary>
    /// <param name="sequence">The sequence to get the value of.</param>
    /// <param name="value">The value of the sequence if found.</param>
    /// <returns>True if the sequence is found, the value of the sequence is returned and the value parameter is set to the value of the sequence.</returns>
    public bool TryGetValue(ReadOnlySpan<byte> sequence, out int value)
    {
        value = 0;
        if (sequence.IsEmpty)
        {
            return false;
        }

        Node? current = this.root;
        foreach (var currentByte in sequence)
        {
            if (current is null || current.Children[currentByte] is null)
            {
                return false;
            }

            current = current.Children[currentByte];
        }

        if (current is null)
        {
            return false;
        }

        value = current.Value;
        return true;
    }

    private class Node(int value)
    {
        public bool IsEndOfSequence { get; set; } = false;

        public int Value { get; set; } = value;

        public Node?[] Children { get; } = new Node[256];
    }
}
