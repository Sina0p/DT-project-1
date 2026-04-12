using System;

public class HashMapIterator<T> : IMyIterator<T>
{
    private readonly ArrayCollection<T>[] _buckets;
    private int _bucketIndex;
    private IMyIterator<T>? _currentIterator;

    public HashMapIterator(ArrayCollection<T>[] buckets)
    {
        _buckets = buckets;
        Reset();
    }

    public bool HasNext()
    {
        if (_currentIterator != null && _currentIterator.HasNext())
            return true;

        int tempBucketIndex = _bucketIndex;

        while (tempBucketIndex < _buckets.Length)
        {
            IMyIterator<T> iterator = _buckets[tempBucketIndex].GetIterator();
            if (iterator.HasNext())
                return true;

            tempBucketIndex++;
        }

        return false;
    }

    public T Next()
    {
        while (_bucketIndex < _buckets.Length)
        {
            if (_currentIterator == null)
                _currentIterator = _buckets[_bucketIndex].GetIterator();

            if (_currentIterator.HasNext())
                return _currentIterator.Next();

            _bucketIndex++;
            _currentIterator = null;
        }

        throw new InvalidOperationException("No more elements.");
    }

    public void Reset()
    {
        _bucketIndex = 0;
        _currentIterator = null;
    }
}