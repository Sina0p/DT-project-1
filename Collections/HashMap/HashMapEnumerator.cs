using System;
using System.Collections;
using System.Collections.Generic;

public class HashMapEnumerator<T> : IEnumerator<T>
{
    private readonly ArrayCollection<T>[] _buckets;
    private int _bucketIndex;
    private IEnumerator<T>? _currentEnumerator;
    private T _current;

    public HashMapEnumerator(ArrayCollection<T>[] buckets)
    {
        _buckets = buckets;
        _bucketIndex = 0;
        _currentEnumerator = null;
        _current = default!;
    }

    public T Current
    {
        get
        {
            if (_currentEnumerator == null)
                throw new InvalidOperationException();

            return _current;
        }
    }

    object IEnumerator.Current => Current!;

    public bool MoveNext()
    {
        while (_bucketIndex < _buckets.Length)
        {
            if (_currentEnumerator == null)
                _currentEnumerator = _buckets[_bucketIndex].GetEnumerator();

            if (_currentEnumerator.MoveNext())
            {
                _current = _currentEnumerator.Current;
                return true;
            }

            _currentEnumerator.Dispose();
            _currentEnumerator = null;
            _bucketIndex++;
        }

        return false;
    }

    public void Reset()
    {
        if (_currentEnumerator != null)
            _currentEnumerator.Dispose();

        _bucketIndex = 0;
        _currentEnumerator = null;
        _current = default!;
    }

    public void Dispose()
    {
        if (_currentEnumerator != null)
            _currentEnumerator.Dispose();
    }
}