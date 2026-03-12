using System;

public class ArrayIterator<T> : IMyIterator<T>
{
    private readonly T[] _items;
    private readonly int _count;
    private int _index;

    public ArrayIterator(T[] items, int count)
    {
        _items = items;
        _count = count;
        _index = 0;
    }

    public bool HasNext()
    {
        return _index < _count;
    }

    public T Next()
    {
        if (!HasNext())
            throw new InvalidOperationException("No more elements.");

        return _items[_index++];
    }

    public void Reset()
    {
        _index = 0;
    }
}