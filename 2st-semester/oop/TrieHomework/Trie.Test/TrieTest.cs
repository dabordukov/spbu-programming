namespace Trie.Test;

public class TrieTest
{
    private static readonly string[] TestData = ["he", "she", "his", "hers"];
    private Trie trie;

    [SetUp]
    public void Setup()
    {
        this.trie = new Trie();
    }

    [Test]
    public void Trie_Add_ReturnsFalseOnExistingWord()
    {
        foreach (var word in TestData)
        {
            Assert.That(this.trie.Add(word), Is.True);
            Assert.That(this.trie.Add(word), Is.False);
        }
    }

    [Test]
    public void Trie_Contains_ReturnsTrueForExistingWord()
    {
        foreach (var word in TestData)
        {
            this.trie.Add(word);
        }

        foreach (var word in TestData)
        {
            Assert.That(this.trie.Contains(word), Is.True);
        }
    }

    [Test]
    public void Trie_Remove_ReturnsFalseOnNonExistingWord()
    {
        foreach (var word in TestData)
        {
            Assert.That(this.trie.Remove(word), Is.False);
        }
    }

    [Test]
    public void Trie_Remove_ReturnsFalseOnNonExistingWordThatIsAPrefixOfExistingWord()
    {
        foreach (var word in TestData)
        {
            this.trie.Add(word);
        }

        Assert.That(this.trie.Remove("her"), Is.False);
        Assert.That(this.trie.Remove("hi"), Is.False);
    }

    [Test]
    public void Trie_HowManyStartsWith_ReturnsCorrectValues()
    {
        foreach (var word in TestData)
        {
            this.trie.Add(word);
        }

        Assert.That(this.trie.HowManyStartsWith("she"), Is.EqualTo(1));
        Assert.That(this.trie.HowManyStartsWith("h"), Is.EqualTo(3));
        Assert.That(this.trie.HowManyStartsWith("he"), Is.EqualTo(2));
        Assert.That(this.trie.HowManyStartsWith("NonExistingWord"), Is.EqualTo(0));
    }

    [Test]
    public void Trie_Size()
    {
        for (int i = 0; i < TestData.Length; i++)
        {
            Assert.That(this.trie.Size, Is.EqualTo(i));
            this.trie.Add(TestData[i]);
        }

        Assert.That(this.trie.Size, Is.EqualTo(TestData.Length));

        for (int i = TestData.Length - 1; i >= 0; i--)
        {
            this.trie.Remove(TestData[i]);
            Assert.That(this.trie.Size, Is.EqualTo(i));
        }
    }
}
