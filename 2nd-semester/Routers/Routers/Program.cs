using Routers;

string helpMessage = $"""
Generate optimal router topology

    Usage: {System.AppDomain.CurrentDomain.FriendlyName} [INPUT FILE] [OUTPUT FILE]
""";

if (args.Length != 2)
{
    Console.WriteLine(helpMessage);
    return 1;
}

if (!File.Exists(args[0]))
{
    Console.WriteLine($"File not found: {args[0]}");
    return 2;
}

Network network = new(args[0]);
Console.WriteLine("\n");
if (!network.MakeOptimal())
{
    Console.Error.WriteLine("Error: network must be connected.");
    return 3;
}

network.PrintOptimalNetwork(args[1]);

return 0;
