namespace MyPriorityQueue.Test;

public class MyPriorityQueueTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void MyPriorityQueueShouldEnqueueDequeueCorrectly()
    {
        MyPriorityQueue queue = new();
        Assert.That(queue.Empty, Is.True);
        queue.Enqueue(1, 1);
        Assert.That(queue.Empty, Is.False);
        Assert.That(queue.Dequeue(), Is.EqualTo(1));
        Assert.That(queue.Empty, Is.True);
    }

    [Test]
    public void MyPriorityQueueShouldDequeueIn_HigherPriorityFirst_Order()
    {
        MyPriorityQueue queue = new();

        queue.Enqueue(2, 5);
        queue.Enqueue(1, 8);
        queue.Enqueue(3, -5);
        Assert.That(queue.Dequeue(), Is.EqualTo(1));
        Assert.That(queue.Dequeue(), Is.EqualTo(2));
        Assert.That(queue.Dequeue(), Is.EqualTo(3));
    }
}
