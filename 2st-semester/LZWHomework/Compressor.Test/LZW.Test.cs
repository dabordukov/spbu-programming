using System.Text;

namespace Compressor.Test
{
    public class LzwTests
    {
        [Test]
        public void EncodeDecodeTest()
        {
            // Arrange: original string data.
            string originalText = "TOBEORNOTTOBEORTOBEORNOT";
            byte[] originalBytes = Encoding.UTF8.GetBytes(originalText);

            // Prepare input stream with original data and output stream for encoded data.
            using var inputStream = new MemoryStream(originalBytes);
            using var encodedStream = new MemoryStream();

            // Act: Encode the input.
            var lzw = new Lzw();
            long[]? frequencies = lzw.Encode(inputStream, encodedStream);
            Assert.That(frequencies, Is.Not.Null, "Frequencies should not be null after encoding.");

            // Prepare to decode by resetting the stream position.
            encodedStream.Position = 0;
            using var decodedStream = new MemoryStream();
            lzw.Decode(encodedStream, decodedStream);

            // Convert decoded bytes to string.
            string decodedText = Encoding.UTF8.GetString(decodedStream.ToArray());

            // Assert: Check that the decoded text matches the original.
            Assert.That(decodedText, Is.EqualTo(originalText), "The decoded text should match the original text.");
        }

        [Test]
        public void EmptyFileTest()
        {
            // Arrange: Create empty input stream.
            using var emptyInput = new MemoryStream();
            using var encodedStream = new MemoryStream();

            var lzw = new Lzw();

            // Act: Encode the empty input.
            long[]? frequencies = lzw.Encode(emptyInput, encodedStream);
            Assert.That(frequencies, Is.Null, "Frequencies should be null after encoding.");

            // Assert: Encoded output should be empty.
            Assert.That(encodedStream.Length, Is.EqualTo(0), "Encoded stream should be empty for an empty input.");
        }
    }
}
