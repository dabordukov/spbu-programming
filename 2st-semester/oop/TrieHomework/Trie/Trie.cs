namespace Trie;

/// <summary>
/// Adds a word to the Trie.
/// Trie data structure.
/// </summary>
public class Trie
{
    private readonly Node root = new();

    /// <summary>
    /// Gets the number of words in the Trie.
    /// </summary>
    public int Size => this.root.NumberOfWords;

    /// <summary>
    /// Adds a word to the Trie.
    /// </summary>
    /// <param name="word">The word to be added to the Trie.</param>
    /// <returns>
    /// True if the word was successfully added to the Trie. <br/> False if the word already exists in the Trie.
    /// </returns>
    public bool Add(string word)
    {
        if (string.IsNullOrEmpty(word))
        {
            return false;
        }

        var node = this.Find(word);
        if (node is not null && node.IsEndOfWord)
        {
            return false;
        }

        var current = this.root;
        current.NumberOfWords++;
        foreach (var character in word)
        {
            if (!current.Children.TryGetValue(character, out Node? value))
            {
                value = new Node();
                current.Children.Add(character, value);
            }

            current = value;
            current.NumberOfWords++;
        }

        current.IsEndOfWord = true;
        return true;
    }

    /// <summary>
    /// Checks if the trie contains the specified word.
    /// </summary>
    /// <param name="word">The word to search for in the trie.</param>
    /// <returns>True if the word is found, otherwise false.</returns>
    public bool Contains(string word)
    {
        if (string.IsNullOrEmpty(word))
        {
            return false;
        }

        var node = this.Find(word);

        if (node is null)
        {
            return false;
        }

        if (node.IsEndOfWord)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Removes the specified word from the trie.
    /// </summary>
    /// <param name="word">The word to remove from the trie.</param>
    /// <returns>True if the word was successfully removed, otherwise false.</returns>
    public bool Remove(string word)
    {
        if (string.IsNullOrEmpty(word))
        {
            return false;
        }

        var node = this.Find(word);
        if (node is null || !node.IsEndOfWord)
        {
            return false;
        }

        var current = this.root;
        current.NumberOfWords--;
        foreach (var character in word)
        {
            current = current.Children[character];
            current.NumberOfWords--;
        }

        node.IsEndOfWord = false;
        return true;
    }

    /// <summary>
    /// Returns the number of words that start with the given prefix.
    /// </summary>
    /// <param name="word">The prefix to search for in the trie.</param>
    /// <returns>The number of words that start with the given prefix.</returns>
    public int HowManyStartsWith(string word)
    {
        if (string.IsNullOrEmpty(word))
        {
            return 0;
        }

        var node = this.Find(word);
        if (node is null)
        {
            return 0;
        }

        return node.NumberOfWords;
    }

    private Node? Find(string word)
    {
        var current = this.root;
        foreach (var character in word)
        {
            if (!current.Children.TryGetValue(character, out Node? value))
            {
                return null;
            }

            current = value;
        }

        return current;
    }

    private class Node
    {
        public bool IsEndOfWord { get; set; } = false;

        public int NumberOfWords { get; set; } = 0;

        public Dictionary<char, Node> Children { get; set; } = [];
    }
}