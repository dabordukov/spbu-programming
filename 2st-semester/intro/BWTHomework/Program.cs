using BurrowsWheeler;

Console.WriteLine("Enter a message to transform: ");

string? message = Console.ReadLine();

if (message is null)
{
    Console.WriteLine("Message is empty");
    return -1;
}

var (encoded, pos) = BWT.Encode(message);

Console.WriteLine($"Encoded message:\n{encoded}\n");

var decoded = BWT.Decode(encoded, pos);

Console.WriteLine($"Decoded message:\n{decoded}\n");

if (message == decoded)
{
    Console.WriteLine("Message is restored correctly.");
}
else
{
    Console.WriteLine("Message is broken");
}

return 0;
