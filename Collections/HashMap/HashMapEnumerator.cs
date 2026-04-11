using System;
using System.Collections;
using System.Collections.Generic;

public class HashMapEnumerator<T> : IEnumerator<T>
{
    private readonly List<T>[] _buckets;
    private int _bucketIndex;
    private int _itemIndex;

    public HashMapEnumerator(List<T>[] buckets)
    {
        _buckets = buckets;
        _bucketIndex = 0;
        _itemIndex = -1;
    }

    public T Current
    {
        get
        {
            if (_bucketIndex >= _buckets.Length || _itemIndex < 0)
                throw new InvalidOperationException();

            return _buckets[_bucketIndex][_itemIndex];
        }
    }

    object IEnumerator.Current => Current!;

    public bool MoveNext()
    {
        while (_bucketIndex < _buckets.Length)
        {
            _itemIndex++;

            if (_itemIndex < _buckets[_bucketIndex].Count)
                return true;

            _bucketIndex++;
            _itemIndex = -1;
        }

        return false;
    }

    public void Reset()
    {
        _bucketIndex = 0;
        _itemIndex = -1;
    }

    public void Dispose() { }
}