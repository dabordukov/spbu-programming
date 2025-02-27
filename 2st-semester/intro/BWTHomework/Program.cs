using BurrowsWheeler;

Console.WriteLine("Enter a message to transform: ");

string? message = Console.ReadLine();

if (message is null)
{
    Console.WriteLine("Message is empty");
    return -1;
}

var (encoded, pos) = BWT.Encode(message);

Console.WriteLine(encoded);

Console.WriteLine(BWT.Decode(encoded, pos));

return 0;
