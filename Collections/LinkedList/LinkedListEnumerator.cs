using System;
using System.Collections;
using System.Collections.Generic;

public class LinkedListEnumerator<T> : IEnumerator<T>
{
    private LinkedListNode<T>? _first;
    private LinkedListNode<T>? _current;

    public LinkedListEnumerator(LinkedListNode<T>? first)
    {
        _first = first;
        _current = null;
    }

    public T Current => _current!.Value;

    object IEnumerator.Current => Current!;

    public bool MoveNext()
    {
        if (_current == null)
        {
            _current = _first;
        }
        else
        {
            _current = _current.Next;
        }

        return _current != null;
    }

    public void Reset()
    {
        _current = null;
    }

    public void Dispose()
    {

    }
}