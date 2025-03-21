string helpMessage = $"""
LZW-Compressor

    Usage: {System.AppDomain.CurrentDomain.FriendlyName} [MODE] [FILE]

    -c      compress
    -u      uncompress
""";

if (args.Length != 2)
{
    Console.WriteLine(helpMessage);
    return 1;
}

switch (args[0])
{
    case "-c":
        var zippedFile = args[1] + ".zipped";
        if (!File.Exists(args[1]))
        {
            Console.WriteLine($"File not found: {args[1]}");
            return 1;
        }

        if (File.Exists(zippedFile))
        {
            Console.WriteLine($"{zippedFile} exists! Delete? [y/N]");
            var userResponse = Console.ReadLine();
            if (userResponse is not ("y" or "Y"))
            {
                return 1;
            }
        }

        Compressor.Compressor.Compress(args[1], zippedFile);
        var oldSize = new System.IO.FileInfo(args[1]).Length;
        var newSize = new System.IO.FileInfo(zippedFile).Length;
        Console.WriteLine($"Old size: {oldSize} Bytes\nNew size: {newSize} Bytes\nCompression ratio: {(float)oldSize / (float)newSize}");
        break;

    case "-u":
        if (!File.Exists(args[1]))
        {
            Console.WriteLine($"File not found: {args[1]}");
            return 1;
        }

        if (args[1].Length <= 7 && args[1][^7..] != ".zipped")
        {
            Console.WriteLine("Not zipped.");
            return 1;
        }

        var unzippedFile = args[1][..^7];

        if (File.Exists(unzippedFile))
        {
            Console.WriteLine($"{unzippedFile} exists! Delete? [y/N]");
            var userResponse = Console.ReadLine();
            if (userResponse is "y" or "Y")
            {
                File.Delete(unzippedFile);
            }
        }

        Compressor.Compressor.Decompress(args[1], unzippedFile);
        break;
}

return 0;
