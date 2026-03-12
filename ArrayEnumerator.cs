using System;
using System.Collections;
using System.Collections.Generic;

public class ArrayEnumerator<T> : IEnumerator<T>
{
    private readonly T[] _items;
    private readonly int _count;
    private int _index = -1;

    public ArrayEnumerator(T[] items, int count)
    {
        _items = items;
        _count = count;
    }

    public T Current
    {
        get
        {
            if (_index < 0 || _index >= _count)
                throw new InvalidOperationException();

            return _items[_index];
        }
    }

    object IEnumerator.Current => Current!;

    public bool MoveNext()
    {
        _index++;
        return _index < _count;
    }

    public void Reset()
    {
        _index = -1;
    }

    public void Dispose()
    {
    }
}