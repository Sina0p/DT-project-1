using System;
using System.Collections.Generic;

public class HashMapIterator<T> : IMyIterator<T>
{
    private readonly List<T>[] _buckets;
    private int _bucketIndex;
    private int _itemIndex;

    public HashMapIterator(List<T>[] buckets)
    {
        _buckets = buckets;
        Reset();
    }

    public bool HasNext()
    {
        int b = _bucketIndex;
        int i = _itemIndex;

        while (b < _buckets.Length)
        {
            i++;

            if (i < _buckets[b].Count)
                return true;

            b++;
            i = -1;
        }

        return false;
    }

    public T Next()
    {
        while (_bucketIndex < _buckets.Length)
        {
            _itemIndex++;

            if (_itemIndex < _buckets[_bucketIndex].Count)
                return _buckets[_bucketIndex][_itemIndex];

            _bucketIndex++;
            _itemIndex = -1;
        }

        throw new InvalidOperationException("No more elements.");
    }

    public void Reset()
    {
        _bucketIndex = 0;
        _itemIndex = -1;
    }
}