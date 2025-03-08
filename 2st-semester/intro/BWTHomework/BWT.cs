namespace BurrowsWheeler;

/// <summary>
/// Burrows-Wheeler Transform.
/// </summary>
public class BWT
{
    /// <summary>
    /// Encode <paramref name="text"/> with Burrows-Wheeler Transform.
    /// </summary>
    /// <param name="text">String to encode.</param>
    /// <returns>BWT-encoded string, index of the last character.</returns>
    public static (string Transform, int EndOfText) Encode(string text)
    {
        string transform = text[^1..];

        int[] suffixes = new int[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            suffixes[i] = i;
        }

        Array.Sort(suffixes, new StringSuffixCompare(text));

        text += "$";
        int endOfText = Array.IndexOf(suffixes, 0);
        suffixes[endOfText] = text.Length;
        for (int i = 0; i < suffixes.Length; i++)
        {
            transform += text[suffixes[i] - 1];
        }

        return (transform, endOfText + 1);
    }

    /// <summary>
    /// Decode <paramref name="transform"/>.
    /// </summary>
    /// <param name="transform">BWT-encoded string.</param>
    /// <param name="endOfText">Last character of the original string.</param>
    /// <returns>Original string.</returns>
    public static string Decode(string transform, int endOfText)
    {
        string text = string.Empty;
        string sorted = string.Concat(transform.OrderBy(c => c));

        int index = 0;
        for (int i = 0; i < transform.Length - 1; i++)
        {
            char next = transform[index];
            text = next + text;
            int occurenceCount = 0;
            for (int j = 0; j <= index; j++)
            {
                if (j == endOfText)
                {
                    continue;
                }

                if (transform[j] == next)
                {
                    occurenceCount++;
                }
            }

            for (int j = 1; j < transform.Length; j++)
            {
                if (sorted[j] == next)
                {
                    occurenceCount--;
                    if (occurenceCount == 0)
                    {
                        index = j;
                        break;
                    }
                }
            }
        }

        return text;
    }

    private class StringSuffixCompare : IComparer<int>
    {
        private readonly string text;

        public StringSuffixCompare(string text)
        {
            this.text = text;
        }

        public int Compare(int x, int y)
        => string.Compare(this.text[x..], this.text[y..]);
    }
}
