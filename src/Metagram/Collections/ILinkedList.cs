namespace Metagram.Collections;

public interface ILinkedList<T> : IList<T>
{
    public LinkedListNode<T>? Last { get; }
    public LinkedListNode<T>? First { get; }

    public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode);
    public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value);
    public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode);
    public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value);
    public void AddFirst(LinkedListNode<T> node);
    public LinkedListNode<T> AddFirst(T value);
    public void AddLast(LinkedListNode<T> node);
    public LinkedListNode<T> AddLast(T value);
    public LinkedListNode<T>? Find(T value);
    public LinkedListNode<T>? FindLast(T value);
    public void Remove(LinkedListNode<T> node);
    public void RemoveFirst();
    public void RemoveLast();
}

public class LinkedListNode<T> 
{

}
