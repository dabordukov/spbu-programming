namespace Routers;

using System.Text.RegularExpressions;

/// <summary>
/// Network topology.
/// </summary>
public class Network
{
    private readonly int[,] graph;

    private readonly int[,] optimalGraph;

    private readonly Dictionary<int, int> aliases = [];

    private readonly Dictionary<int, int> reverseAliases = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="Network"/> class.
    /// </summary>
    /// <param name="filename">File with the network topology.</param>
    public Network(string filename)
    {
        if (!File.Exists(filename))
        {
            throw new ArgumentException($"File not found: {filename}");
        }

        this.AliasNodes(filename);
        this.reverseAliases = this.aliases.ToDictionary(x => x.Value, x => x.Key);

        this.graph = new int[this.GraphSize, this.GraphSize];
        this.optimalGraph = new int[this.GraphSize, this.GraphSize];
        using var input = new StreamReader(filename, System.Text.Encoding.UTF8);
        string? line;

        while ((line = input.ReadLine()) != null)
        {
            var parts = line.Split(':');
            if (!int.TryParse(parts[0], out int currentNumber) || currentNumber < 0)
            {
                throw new InvalidGraph("Can't parse node");
            }

            var currentNode = this.aliases[currentNumber];

            var neighbors = parts[1].Split(',');
            for (int i = 0; i < neighbors.Length; i++)
            {
                Regex r = new(@" *(?<neighbor>\d+) *\( *(?<weight>\d+ *) *\) *", RegexOptions.Compiled);
                Match m = r.Match(neighbors[i]);
                if (!m.Success)
                {
                    throw new InvalidGraph("Can't parse neighbors");
                }

                if (!int.TryParse(m.Groups[1].Value, out int neighborParsed) || neighborParsed < 0)
                {
                    throw new InvalidGraph("Invalid neighbor");
                }

                var neighbor = this.aliases[neighborParsed];

                if (!int.TryParse(m.Groups[2].Value, out int weight) || weight < 0)
                {
                    throw new InvalidGraph("Invalid weight");
                }

                if (this.graph[currentNode, neighbor] != weight)
                {
                    if (this.graph[currentNode, neighbor] != 0)
                    {
                        throw new InvalidGraph($"Invalid graph: [{currentNumber}]<= {weight} =>[{neighbor}] but [{neighbor}]<= {this.graph[neighbor, currentNode]} =>[{currentNode}]");
                    }

                    this.graph[currentNode, neighbor] = weight;
                }

                if (this.graph[neighbor, currentNode] != weight)
                {
                    if (this.graph[neighbor, currentNode] != 0)
                    {
                        throw new InvalidGraph($"Invalid graph: [{neighbor}]<= {weight} =>[{currentNode}] but [{currentNumber}]<= {this.graph[currentNode, neighbor]} =>[{neighbor}]");
                    }

                    this.graph[neighbor, currentNode] = weight;
                }
            }
        }
    }

    private int GraphSize => this.aliases.Count;

    /// <summary>
    /// Finds the optimal network topology.
    /// </summary>
    /// <returns>True if all network nodes are connected, otherwise false.</returns>
    public bool MakeOptimal()
    {
        bool[] used = new bool[this.GraphSize];
        used[0] = true;
        PriorityQueue<(int A, int B), int> queue = new();
        for (int i = 1; i < this.GraphSize; i++)
        {
            if (this.graph[0, i] != 0)
            {
                queue.Enqueue(new(0, i), -this.graph[0, i]);
            }
        }

        for (int i = 0; i < this.GraphSize - 1; i++)
        {
            while (queue.Count > 0)
            {
                var edge = queue.Dequeue();
                if (!used[edge.B])
                {
                    used[edge.B] = true;
                    for (int j = 0; j < this.GraphSize; j++)
                    {
                        if (!used[j] && this.graph[edge.B, j] != 0)
                        {
                            queue.Enqueue(new(edge.B, j), -this.graph[edge.B, j]);
                        }
                    }

                    this.optimalGraph[edge.A, edge.B] = this.graph[edge.A, edge.B];
                    this.optimalGraph[edge.B, edge.A] = this.graph[edge.A, edge.B];

                    break;
                }
            }
        }

        for (int i = 0; i < this.GraphSize; i++)
        {
            if (!used[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Prints the network topology.
    /// </summary>
    /// <param name="writer">Stream to write.</param>
    public void PrintNetwork(StreamWriter writer)
    {
        for (int i = 0; i < this.GraphSize; i++)
        {
            string buffer = $"{this.reverseAliases[i]}: ";
            for (int j = 0; j < this.GraphSize; j++)
            {
                if (this.graph[i, j] != 0)
                {
                    buffer += $"{this.reverseAliases[j]} ({this.graph[i, j]}), ";
                }
            }

            buffer = buffer[..^2];
            writer.WriteLine(buffer);
        }
    }

    /// <summary>
    /// Prints the optimal network topology.
    /// </summary>
    /// <param name="filename">File to write optimal topology.</param>
    public void PrintOptimalNetwork(string filename)
    {
        using var output = new StreamWriter(filename);
        for (int i = 0; i < this.GraphSize; i++)
        {
            string buffer = string.Empty;
            for (int j = i; j < this.GraphSize; j++)
            {
                if (this.optimalGraph[i, j] != 0)
                {
                    buffer += $"{this.reverseAliases[j]} ({this.optimalGraph[i, j]}), ";
                }
            }

            if (buffer != string.Empty)
            {
                buffer = buffer[..^2];
                output.WriteLine($"{this.reverseAliases[i]}: " + buffer);
            }
        }
    }

    private void AliasNodes(string filename)
    {
        using var input = new StreamReader(filename, System.Text.Encoding.UTF8);

        string? line;
        while ((line = input.ReadLine()) != null)
        {
            var parts = line.Split(':');
            if (!int.TryParse(parts[0], out int currentNode) || currentNode < 0)
            {
                throw new InvalidGraph("Can't parse node");
            }

            this.aliases.TryAdd(currentNode, this.aliases.Count);

            var neighbors = parts[1].Split(',');
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!int.TryParse(neighbors[i][..neighbors[i].IndexOf('(')], out int neighbor) || neighbor < 0)
                {
                    throw new InvalidGraph("Invalid neighbor");
                }

                this.aliases.TryAdd(neighbor, this.aliases.Count);
            }
        }
    }

    private record Edge(int A, int B, int Weight);

    /// <summary>
    /// Invalid network topology format.
    /// </summary>
    public class InvalidGraph : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGraph"/> class.
        /// </summary>
        public InvalidGraph()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGraph"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidGraph(string message)
        : base(message)
        {
        }
    }
}
