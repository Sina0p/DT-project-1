using System;

public class LinkedListIterator<T> : IMyIterator<T>
{
    private LinkedListNode<T>? _start;
    private LinkedListNode<T>? _current;

    public LinkedListIterator(LinkedListNode<T>? first)
    {
        _start = first;
        _current = first;
    }

    public bool HasNext()
    {
        return _current != null;
    }

    public T Next()
    {
        if (_current == null)
            throw new InvalidOperationException("No more elements.");

        var value = _current.Value;
        _current = _current.Next;

        return value;
    }

    public void Reset()
    {
        _current = _start;
    }
}