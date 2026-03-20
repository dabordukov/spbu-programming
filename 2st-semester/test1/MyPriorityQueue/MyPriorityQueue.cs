namespace MyPriorityQueue;

/// <summary>
/// Implements a priority queue.
/// </summary>
public class MyPriorityQueue
{
    /// <summary>
    /// Gets number of elements in PriorityQueue.
    /// </summary>
    private int size = 0;

    private Node? first = null;
    private Node? last = null;

    /// <summary>
    /// Gets a value indicating whether the PriorityQueue is empty.
    /// </summary>
    public bool Empty =>
        this.size == 0;

    /// <summary>
    /// Enqueues the element in PriorityQueue.
    /// </summary>
    /// <param name="value">The value of element to enqueue.</param>
    /// <param name="priority">The priority of the element.</param>
    public void Enqueue(int value, int priority)
    {
        if (this.size == 0)
        {
            this.first = new Node(value, priority, null, null);
            this.last = this.first;
            this.size++;
            return;
        }

        var node = this.first;
        while (node is not null && node.Priority >= priority)
        {
            node = node.Next;
        }

        if (node is null)
        {
            var newNode = new Node(value, priority, null, this.last);
            if (this.last is null)
            {
                return;
            }

            this.last.Next = newNode;
            this.last = newNode;
        }
        else
        {
            var newNode = new Node(value, priority, node, node.Previous);
            if (node.Previous is null)
            {
                this.first = newNode;
                node.Previous = newNode;
                this.size++;
                return;
            }

            node.Previous.Next = newNode;
            node.Previous = newNode;
        }

        this.size++;
    }

    /// <summary>
    /// Dequeues the element with the highest priority from PriorityQueue.
    /// </summary>
    /// <returns>The value of the dequeued element.</returns>
    public int Dequeue()
    {
        if (this.first is null)
        {
            throw new Exception("PriorityQueue is empty");
        }

        var value = this.first.Value;
        this.first = this.first.Next;
        if (this.first is not null)
        {
            this.first.Previous = null;
        }

        this.size--;
        return value;
    }

    private record Node(int Value, int Priority, Node? Next, Node? Previous)
    {
        public int Value { get; init; } = Value;

        public int Priority { get; init; } = Priority;

        public Node? Next { get; set; } = Next;

        public Node? Previous { get; set; } = Previous;
    }
}
